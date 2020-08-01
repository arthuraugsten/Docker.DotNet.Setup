using System.Collections.Generic;

namespace Docker.DotNet.Setup.Models
{
    public sealed class PortOptions
    {
        public IEnumerable<Binding> Ports { get; set; }
        public bool PublishAllPorts { get; set; } = true;
    }
}