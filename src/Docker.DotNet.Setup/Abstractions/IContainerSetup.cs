using System.Threading.Tasks;

namespace Docker.DotNet.Setup.Abstractions
{
    public interface IContainerSetup
    {
        bool IsReady { get; }

        Task ConfigureAsync();
        Task DiscardAsync();
    }
}