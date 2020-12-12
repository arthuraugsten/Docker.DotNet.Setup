using Docker.DotNet.Setup.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Docker.DotNet.Setup
{
    public sealed class ContainerPool : IContainerPool
    {
        private readonly List<IContainerSetup> _setups = new List<IContainerSetup>();
        private readonly IClientFacade _client;
        private readonly INetworkSetup _network;

        public ContainerPool(IClientFacade client, INetworkSetup network)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));

            _client = client;
            _network = network;
        }

        public IContainerPool Add<TSetup>(TSetup setup) where TSetup : IContainerSetup
        {
            if (setup == null)
                return this;

            _setups.Add(setup);

            return this;
        }

        public IContainerPool Add<TSetup>() where TSetup : IContainerSetup, new()
            => Add(new TSetup());

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

            if (_network?.ShouldRemoveNetworkOnExit ?? false)
            {
                var networkId = await _client.GetNetworkIdAsync(_network.Name).ConfigureAwait(false);
                await _client.DeleteNetworkAsync(networkId).ConfigureAwait(false);
            }
        }
    }
}