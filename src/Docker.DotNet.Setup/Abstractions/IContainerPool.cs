using System.Threading.Tasks;

namespace Docker.DotNet.Setup.Abstractions
{
    public interface IContainerPool
    {
        IContainerPool Add<TSetup>(TSetup setup) where TSetup : IContainerSetup;
        IContainerPool Add<TSetup>() where TSetup : IContainerSetup, new();
        Task ConfigureAllAsync();
        Task DiscardAllAsync();
    }
}