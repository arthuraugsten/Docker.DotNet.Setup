using System;

namespace Docker.DotNet.Setup.Models
{
    public sealed class NetworkOptions
    {
        public string Name { get; set; } = $"setup-{Guid.NewGuid().ToString()}";
        public bool RemoveNetworkOnExit { get; set; } = true;
    }
}