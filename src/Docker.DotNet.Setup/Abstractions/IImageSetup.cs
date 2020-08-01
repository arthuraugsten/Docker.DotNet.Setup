using System.Threading.Tasks;

namespace Docker.DotNet.Setup.Abstractions
{
    public interface IImageSetup
    {
        bool IsReady { get; }

        Task ConfigureAsync();
        Task DiscardAsync();
    }
}