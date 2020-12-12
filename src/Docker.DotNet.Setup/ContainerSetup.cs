using Docker.DotNet.Setup.Abstractions;
using Docker.DotNet.Setup.Models;
using System;
using System.Threading.Tasks;

namespace Docker.DotNet.Setup
{
    public abstract class ContainerSetup : IContainerSetup
    {
        protected string ContainerId { get; private set; }

        public bool IsReady { get; private set; }

        protected abstract ContainerOptions Options { get; }
        protected abstract Task WaitContainerAsync();

        protected virtual async Task DownloadImageAsync(IClientFacade client)
            => await client.DownloadImageAsync(Options.ImageName, Options.ImageTag);

        protected async Task CreateContainerAsync(IClientFacade client, INetworkSetup network)
        {
            var container = await client.GetExistingContainerAsync(Options.Name);

            ContainerId = container is null
                ? await client.CreateContainerAsync(Options, network)
                : container.Id;
        }

        protected void DefineAsReady()
        {
            if (string.IsNullOrEmpty(ContainerId))
                throw new InvalidOperationException("You must create container before set it as ready.");

            IsReady = true;
        }

        public virtual async Task ConfigureAsync(IClientFacade client, INetworkSetup network)
        {
            try
            {
                if (network != default)
                    await network.CreateNetworkAsync();

                var existingContainer = await client.GetExistingContainerAsync(Options.Name);

                if (existingContainer == default)
                {
                    await DownloadImageAsync(client);
                    await CreateContainerAsync(client, network);
                }
                else
                {
                    ContainerId = existingContainer.Id;
                }

                if (!existingContainer?.IsRunning ?? true)
                    await client.StartContainerAsync(ContainerId);

                await WaitContainerAsync();
                DefineAsReady();
            }
            catch (DockerApiException ex)
            {
                if (ex.Message.Contains("code=Conflict"))
                {
                    await WaitContainerAsync();
                    DefineAsReady();
                }
                else
                {
                    throw new InvalidOperationException("Something unexpected happened during image setup.", ex);
                }
            }
        }

        public async Task DiscardAsync(IClientFacade client)
        {
            if (ContainerId == null || !Options.RemoveContainerOnExit)
                return;

            await client.KillContainerAsync(ContainerId);
            await client.RemoveContainerAsync(ContainerId);
        }
    }
}