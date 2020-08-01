using Docker.DotNet.Models;
using Docker.DotNet.Setup.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Docker.DotNet.Setup.Mappers
{
    internal static class NetworkConfigMapper
    {
        public static async Task<NetworkingConfig> MapFromAsync(INetworkSetup setup)
        {
            if (setup is null) return null;

            return new NetworkingConfig
            {
                EndpointsConfig = new Dictionary<string, EndpointSettings>
                {
                    { setup.Name , new EndpointSettings { NetworkID = await setup.GetNetworkIdAsync().ConfigureAwait(false) } }
                }
            };
        }
    }
}