using Docker.DotNet.Setup.Abstractions;
using Docker.DotNet.Setup.Models;
using System;
using System.Threading.Tasks;

namespace Docker.DotNet.Setup
{
    public abstract class NetworkSetup : INetworkSetup
    {
        private readonly NetworkOptions _options;
        private readonly IClientFacade _client;

        public NetworkSetup(IClientFacade client, NetworkOptions options)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (options is null) throw new ArgumentNullException(nameof(options));

            _client = client;
            _options = options;
        }

        public NetworkSetup(IClientFacade client, Func<NetworkOptions> options)
            : this(client, options?.Invoke()) { }

        protected string Id { get; private set; }

        public string Name => _options.Name;
        public bool ShouldRemoveNetworkOnExit => _options.RemoveNetworkOnExit;

        public async Task CreateNetworkAsync()
            => Id = await _client.CreateNetworkAsync(Name).ConfigureAwait(false);
    }
}