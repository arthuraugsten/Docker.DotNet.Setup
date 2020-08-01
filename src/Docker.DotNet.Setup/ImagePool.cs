using Docker.DotNet.Setup.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Docker.DotNet.Setup
{
    public sealed class ImagePool<TNetwork> : IImagePool where TNetwork : INetworkSetup
    {
        private readonly List<IImageSetup> _setups = new List<IImageSetup>();
        private readonly DockerClient _client;
        private readonly INetworkSetup _network;

        public ImagePool(INetworkSetup network)
        {
            if (network is null) throw new ArgumentNullException(nameof(network));

            _network = network;

            try
            {
                _client = ClientFactory.CreateClient();
            }
            catch (DockerApiException ex)
            {
                throw new InvalidOperationException("Something was wrong on docker API communication.", ex);
            }
        }

        public IImagePool Add<TSetup>(TSetup setup) where TSetup : IImageSetup
        {
            if (setup == null)
                return this;

            _setups.Add(setup);

            return this;
        }

        public async Task ConfigureAllAsync()
        {
            foreach (var setup in _setups)
                await setup.ConfigureAsync().ConfigureAwait(false);
        }

        public async Task DiscardAllAsync()
        {
            var discardTasks = _setups.Select(s =>
                Task.Factory.StartNew(async () => await s.DiscardAsync().ConfigureAwait(false)));

            await Task.WhenAll(discardTasks).ConfigureAwait(false);

            if (_network.ShouldRemoveNetworkOnExit)
            {
                var networkId = await _network.GetNetworkIdAsync().ConfigureAwait(false);
                await _client.Networks.DeleteNetworkAsync(networkId).ConfigureAwait(false);
            }
        }
    }
}