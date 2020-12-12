using Docker.DotNet.Setup.Abstractions;
using Docker.DotNet.Setup.Models;
using System;
using System.Threading.Tasks;

namespace Docker.DotNet.Setup
{
    public abstract class NetworkSetup : INetworkSetup
    {
        private readonly IClientFacade _client;

        public NetworkSetup(IClientFacade client)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));

            _client = client;
        }

        protected string Id { get; private set; }
        protected abstract NetworkOptions Options { get; }

        public string Name => Options.Name;
        public bool ShouldRemoveNetworkOnExit => Options.RemoveNetworkOnExit;

        public async Task CreateNetworkAsync()
            => Id = await _client.CreateNetworkAsync(Name);
    }
}