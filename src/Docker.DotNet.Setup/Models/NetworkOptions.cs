using System;

namespace Docker.DotNet.Setup.Models
{
    public sealed class NetworkOptions
    {
        public string Name { get; set; } = $"setup-{Guid.NewGuid()}";
        public bool RemoveNetworkOnExit { get; set; } = true;
    }
}