using Docker.DotNet.Models;
using Docker.DotNet.Setup.Abstractions;
using System;
using System.Collections.Generic;

namespace Docker.DotNet.Setup.Mappers
{
    public static class NetworkConfigMapper
    {
        public static NetworkingConfig MapFrom(INetworkSetup setup, string networkId)
        {
            if (setup is null) throw new ArgumentNullException(nameof(setup));
            if (string.IsNullOrEmpty(networkId)) throw new ArgumentNullException(nameof(networkId));

            return new NetworkingConfig
            {
                EndpointsConfig = new Dictionary<string, EndpointSettings>
                {
                    { setup.Name , new EndpointSettings { NetworkID = networkId } }
                }
            };
        }
    }
}