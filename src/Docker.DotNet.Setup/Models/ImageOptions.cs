using System.Collections.Generic;

namespace Docker.DotNet.Setup.Models
{
    public sealed class ImageOptions
    {
        public IList<string> Environment { get; } = new List<string>();
        public PortOptions PortsOptions { get; set; }
        public IEnumerable<MountOptions> Mounts { get; } = new List<MountOptions>();
        public bool RemoveContainerOnExit { get; set; } = true;
    }
}