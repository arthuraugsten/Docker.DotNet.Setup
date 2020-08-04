using System.Threading.Tasks;

namespace Docker.DotNet.Setup.Abstractions
{
    public interface INetworkSetup
    {
        string Name { get; }
        bool ShouldRemoveNetworkOnExit { get; }

        Task CreateNetworkAsync();
    }
}