using System.Collections.Generic;

namespace Docker.DotNet.Setup.Models
{
    public sealed class ContainerOptions
    {
        public string ImageName { get; set; }
        public string ImageTag { get; set; }
        public string Name { get; set; }
        public bool RemoveContainerOnExit { get; set; } = true;
        public IList<string> Environment { get; set; }
        public PortOptions PortsOptions { get; set; }
        public IEnumerable<MountOptions> Mounts { get; set; }
    }
}