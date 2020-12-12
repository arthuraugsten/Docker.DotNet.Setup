using Docker.DotNet.Setup;
using Docker.DotNet.Setup.Abstractions;
using Docker.DotNet.Setup.Models;

namespace TestSamples
{
    public sealed class AppNetworkSetup : NetworkSetup
    {
        public AppNetworkSetup(IClientFacade client)
            : base(client) { }

        protected override NetworkOptions Options { get; } = new NetworkOptions();
    }
}