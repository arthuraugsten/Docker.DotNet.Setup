using System.Threading.Tasks;

namespace Docker.DotNet.Setup.Abstractions
{
    public interface IImagePool
    {
        IImagePool Add<TSetup>(TSetup setup) where TSetup : IImageSetup;
        Task ConfigureAllAsync();
        Task DiscardAllAsync();
    }
}