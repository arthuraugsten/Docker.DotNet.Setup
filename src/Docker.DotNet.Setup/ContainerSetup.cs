using Docker.DotNet.Setup.Abstractions;
using Docker.DotNet.Setup.Models;
using System;
using System.Threading.Tasks;

namespace Docker.DotNet.Setup
{
    public abstract class ContainerSetup : IContainerSetup
    {
        private readonly IClientFacade _client;
        private readonly INetworkSetup _network;
        private readonly ContainerOptions _options;

        public ContainerSetup(IClientFacade client,
            ContainerOptions options,
            INetworkSetup network = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (options is null) throw new ArgumentNullException(nameof(options));

            _client = client;
            _options = options;
            _network = network;
        }

        public ContainerSetup(IClientFacade client,
            Func<ContainerOptions> options,
            INetworkSetup network = default) : this(client, options?.Invoke(), network)
        { }

        protected string ContainerId { get; private set; }

        public bool IsReady { get; private set; }

        protected abstract Task WaitContainerAsync();

        protected virtual async Task DownloadImageAsync()
            => await _client.DownloadImageAsync(_options.ImageName, _options.ImageTag).ConfigureAwait(false);

        protected async Task CreateContainerAsync()
        {
            var container = await _client.GetExistingContainerAsync(_options.Name).ConfigureAwait(false);

            ContainerId = container is null
                ? await _client.CreateContainerAsync(_options, _network).ConfigureAwait(false)
                : container.Id;
        }

        protected void DefineAsReady()
        {
            if (string.IsNullOrEmpty(ContainerId))
                throw new InvalidOperationException("You must create container before set it as ready.");

            IsReady = true;
        }

        public async Task ConfigureAsync()
        {
            try
            {
                if (_network != default)
                    await _network.CreateNetworkAsync().ConfigureAwait(false);

                if (await _client.IsRunningAsync(_options.Name).ConfigureAwait(false))
                    DefineAsReady();

                await DownloadImageAsync().ConfigureAwait(false);
                await CreateContainerAsync().ConfigureAwait(false);

                await _client.StartContainerAsync(ContainerId).ConfigureAwait(false);

                await WaitContainerAsync().ConfigureAwait(false);
                DefineAsReady();
            }
            catch (DockerApiException ex)
            {
                if (ex.Message.Contains("code=Conflict"))
                {
                    await WaitContainerAsync().ConfigureAwait(false);
                    DefineAsReady();
                }

                else
                {
                    throw new InvalidOperationException("Something unexpected happened during image setup.", ex);
                }
            }
        }

        public async Task DiscardAsync()
        {
            if (ContainerId == null || !_options.RemoveContainerOnExit)
                return;

            await _client.KillContainerAsync(ContainerId).ConfigureAwait(false);
            await _client.RemoveContainerAsync(ContainerId).ConfigureAwait(false);
        }
    }
}