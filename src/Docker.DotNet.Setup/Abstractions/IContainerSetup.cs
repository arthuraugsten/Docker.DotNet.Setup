using System.Threading.Tasks;

namespace Docker.DotNet.Setup.Abstractions
{
    public interface IContainerSetup
    {
        bool IsReady { get; }

        Task ConfigureAsync(IClientFacade client, INetworkSetup network);
        Task DiscardAsync(IClientFacade client);
    }
}