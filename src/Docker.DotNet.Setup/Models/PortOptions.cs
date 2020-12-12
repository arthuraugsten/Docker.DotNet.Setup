using System;
using System.Collections.Generic;

namespace Docker.DotNet.Setup.Models
{
    public sealed class PortOptions
    {
        public PortOptions(IEnumerable<Binding> ports, bool publishAllPorts = true)
        {
            Ports = ports ?? throw new ArgumentNullException(nameof(ports));
            PublishAllPorts = publishAllPorts;
        }

        public IEnumerable<Binding> Ports { get; }
        public bool PublishAllPorts { get; }
    }
}