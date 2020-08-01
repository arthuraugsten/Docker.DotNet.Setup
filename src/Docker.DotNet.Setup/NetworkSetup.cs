using Docker.DotNet.Models;
using Docker.DotNet.Setup.Abstractions;
using Docker.DotNet.Setup.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Docker.DotNet.Setup
{
    public abstract class NetworkSetup : INetworkSetup
    {
        private readonly NetworkOptions _options;
        private DockerClient _client;

        protected NetworkSetup(DockerClient client)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));

            _client = client;
            _options = GetNetworkOptions();
        }

        protected string Id { get; private set; }

        protected abstract NetworkOptions GetNetworkOptions();

        public string Name => _options.Name;
        public bool ShouldRemoveNetworkOnExit => _options.RemoveNetworkOnExit;

        public async Task CreateNetworkAsync()
        {
            var networks = await _client.Networks.ListNetworksAsync().ConfigureAwait(false);

            if (networks.FirstOrDefault(n => n.Name == Name) is NetworkResponse response)
            {
                Id = response.ID;
                return;
            }

            var network = await _client.Networks
                .CreateNetworkAsync(new NetworksCreateParameters
                {
                    Name = Name,
                    Driver = "bridge"
                }).ConfigureAwait(false);

            Id = network.ID;
        }

        public async Task<string> GetNetworkIdAsync()
        {
            if (!string.IsNullOrEmpty(Id))
                return Id;

            var networks = await _client.Networks.ListNetworksAsync().ConfigureAwait(false);

            Id = networks.FirstOrDefault(n => n.Name == Name)?.ID;

            return Id;
        }
    }
}