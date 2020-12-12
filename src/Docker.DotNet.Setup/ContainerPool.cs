using Docker.DotNet.Setup.Abstractions;
using System;
using System.Collections.Generic;
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
                await setup.ConfigureAsync(_client, _network);
        }

        public async Task DiscardAllAsync()
        {
            foreach (var setup in _setups)
                await setup.DiscardAsync(_client);

            if (_network?.ShouldRemoveNetworkOnExit ?? false)
            {
                var networkId = await _client.GetNetworkIdAsync(_network.Name);
                await _client.DeleteNetworkAsync(networkId);
            }
        }
    }
}