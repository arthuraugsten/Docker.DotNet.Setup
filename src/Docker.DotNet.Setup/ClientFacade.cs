using Docker.DotNet.Models;
using Docker.DotNet.Setup.Abstractions;
using Docker.DotNet.Setup.Mappers;
using Docker.DotNet.Setup.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Docker.DotNet.Setup
{
    public sealed class ClientFacade : IClientFacade
    {
        private readonly DockerClient _client;

        private bool _disposed;

        public ClientFacade(DockerClient client)
            => _client = client;

        public async Task<string> CreateContainerAsync(ContainerOptions options, INetworkSetup network = default)
        {
            if (options is null) throw new ArgumentNullException(nameof(options));

            var image = ImageOptionsMapper.MapFrom(options);

            image.Name = options.ImageName;
            image.Image = $"{options.ImageName}:{options.ImageTag}";

            if (network != default)
                image.NetworkingConfig = NetworkConfigMapper.MapFrom(network, await GetNetworkIdAsync(network.Name).ConfigureAwait(false));

            var response = await _client.Containers.CreateContainerAsync(image).ConfigureAwait(false);
            return response.ID;
        }

        public async Task<string> CreateNetworkAsync(string name)
        {
            const string driverType = "bridge";

            var networks = await _client.Networks.ListNetworksAsync().ConfigureAwait(false);

            if (networks.FirstOrDefault(n => n.Name == name) is NetworkResponse response)
                return response.ID;

            var network = await _client.Networks
                .CreateNetworkAsync(new NetworksCreateParameters
                {
                    Name = name,
                    Driver = driverType
                }).ConfigureAwait(false);

            return network.ID;
        }

        public async Task DeleteNetworkAsync(string networkId)
            => await _client.Networks.DeleteNetworkAsync(networkId).ConfigureAwait(false);

        public async Task DownloadImageAsync(string name, string tag)
            => await _client.Images
                .CreateImageAsync(
                    new ImagesCreateParameters { FromImage = name, Tag = tag },
                    new AuthConfig(),
                    new Progress<JSONMessage>()
                )
               .ConfigureAwait(false);

        public async Task<string> GetNetworkIdAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
                return default;

            var networks = await _client.Networks.ListNetworksAsync().ConfigureAwait(false);

            return networks.FirstOrDefault(n => n.Name == name)?.ID;
        }

        public async Task<ContainerInfo> GetExistingContainerAsync(string name)
        {
            var containers = await _client.Containers
                .ListContainersAsync(new ContainersListParameters { All = true })
                .ConfigureAwait(false);

            var selectedContainer = containers?.FirstOrDefault(c => c.Names.Any(n => n == $"/{name}"));

            return selectedContainer is null ? default : new ContainerInfo(selectedContainer.ID, selectedContainer.State);
        }

        public async Task<bool> IsRunningAsync(string name)
        {
            const string runningState = "running";

            try
            {
                (var _, var state) = await GetExistingContainerAsync(name).ConfigureAwait(false);
                return state == runningState;
            }
            catch (DockerApiException)
            {
                return false;
            }
        }

        public async Task KillContainerAsync(string containerId)
            => await _client.Containers
                .KillContainerAsync(containerId, new ContainerKillParameters())
                .ConfigureAwait(false);

        public async Task RemoveContainerAsync(string containerId)
            => await _client.Containers
                .RemoveContainerAsync(containerId, new ContainerRemoveParameters { Force = true })
                .ConfigureAwait(false);

        public async Task StartContainerAsync(string containerId)
            => await _client.Containers
                .StartContainerAsync(containerId, new ContainerStartParameters())
                .ConfigureAwait(false);

        public void Dispose()
        {
            if (_disposed)
                return;

            _client?.Dispose();
            GC.SuppressFinalize(this);
            _disposed = true;
        }
    }
}