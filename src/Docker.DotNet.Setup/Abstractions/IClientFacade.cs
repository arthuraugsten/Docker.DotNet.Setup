using Docker.DotNet.Setup.Models;
using System;
using System.Threading.Tasks;

namespace Docker.DotNet.Setup.Abstractions
{
    public interface IClientFacade : IDisposable
    {
        Task<string> CreateContainerAsync(ContainerOptions options, INetworkSetup network = default);
        Task<string> CreateNetworkAsync(string name);
        Task DeleteNetworkAsync(string networkId);
        Task DownloadImageAsync(string name, string tag);
        Task<ContainerInfo> GetExistingContainerAsync(string name);
        Task<string> GetNetworkIdAsync(string name);
        Task<bool> IsRunningAsync(string name);
        Task KillContainerAsync(string containerId);
        Task RemoveContainerAsync(string containerId);
        Task StartContainerAsync(string containerId);
    }
}