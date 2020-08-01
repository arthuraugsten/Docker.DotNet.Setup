using Docker.DotNet.Models;
using Docker.DotNet.Setup.Abstractions;
using Docker.DotNet.Setup.Mappers;
using Docker.DotNet.Setup.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Docker.DotNet.Setup
{
    public abstract class ImageSetup : IImageSetup
    {
        private readonly DockerClient _client;
        private readonly INetworkSetup _network;
        private readonly ImageOptions _imageOptions;

        public ImageSetup(DockerClient client, INetworkSetup network = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));

            _client = client;
            _network = network;
            _imageOptions = GetImageOptions();
        }

        public bool IsReady { get; private set; }

        protected abstract string ContainerName { get; }
        protected abstract string ImageName { get; }
        protected abstract string ImageTag { get; }

        protected string ContainerId { get; private set; }

        protected abstract ImageOptions GetImageOptions();
        protected abstract Task WaitContainerAsync();

        protected async Task<ContainerInfo> GetExistingContainer()
        {
            var containers = await _client.Containers
                .ListContainersAsync(new ContainersListParameters { All = true })
                .ConfigureAwait(false);

            var selectedContainer = containers.FirstOrDefault(c => c.Names.Any(n => n == $"/{ContainerName}"));

            if (selectedContainer is null) return default;

            return new ContainerInfo(selectedContainer.ID, selectedContainer.State);
        }

        protected async Task<bool> IsRunningAsync()
        {
            try
            {
                var (_, state) = await GetExistingContainer().ConfigureAwait(false);
                return state == "running";
            }
            catch (DockerApiException ex)
            {
                Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
                return false;
            }
        }

        protected virtual async Task DownloadImageAsync()
        {
            await _client.Images
                .CreateImageAsync(new ImagesCreateParameters { FromImage = ImageName, Tag = ImageTag },
                    new AuthConfig(),
                    new Progress<JSONMessage>())
               .ConfigureAwait(false);
        }

        protected async Task ConfigureImageAsync()
        {
            var container = await GetExistingContainer().ConfigureAwait(false);

            if (container is null)
            {
                var image = ImageOptionsMapper.MapFrom(_imageOptions);

                image.Name = ImageName;
                image.Image = $"{ImageName}{ImageTag}";

                if (_network != default)
                    image.NetworkingConfig = await NetworkConfigMapper.MapFromAsync(_network).ConfigureAwait(false);

                var response = await _client.Containers.CreateContainerAsync(image).ConfigureAwait(false);
                ContainerId = response.ID;
            }
            else
            {
                ContainerId = container.Id;
            }
        }

        protected void DefineAsReady() => IsReady = true;

        public async Task ConfigureAsync()
        {
            try
            {
                if (_network != default)
                    await _network.CreateNetworkAsync().ConfigureAwait(false);

                if (await IsRunningAsync().ConfigureAwait(false))
                    DefineAsReady();

                await DownloadImageAsync().ConfigureAwait(false);
                await ConfigureImageAsync().ConfigureAwait(false);

                await _client.Containers
                    .StartContainerAsync(ContainerId, new ContainerStartParameters())
                    .ConfigureAwait(false);

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

                else throw new InvalidOperationException("Something unexpected happened during image setup.", ex);
            }
        }

        public async Task DiscardAsync()
        {
            if (ContainerId == null || _imageOptions.RemoveContainerOnExit)
                return;

            await _client.Containers
                .KillContainerAsync(ContainerId, new ContainerKillParameters())
                .ConfigureAwait(false);

            await _client.Containers
                .RemoveContainerAsync(ContainerId, new ContainerRemoveParameters { Force = true })
                .ConfigureAwait(false);
        }
    }
}