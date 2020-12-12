using System;

namespace Docker.DotNet.Setup.Models
{
    public sealed class NetworkOptions
    {
        public NetworkOptions() { }

        public NetworkOptions(string name)
            => Name = name;

        public NetworkOptions(string name, bool removeNetworkOnExit) : this(name)
            => RemoveNetworkOnExit = removeNetworkOnExit;

        public string Name { get; } = $"setup-{Guid.NewGuid()}";
        public bool RemoveNetworkOnExit { get; } = true;
    }
}