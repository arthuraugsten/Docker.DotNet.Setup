using Docker.DotNet.Models;
using Docker.DotNet.Setup.Models;
using System.Collections.Generic;
using System.Linq;

namespace Docker.DotNet.Setup.Mappers
{
    public static class ImageOptionsMapper
    {
        public static CreateContainerParameters MapFrom(ContainerOptions options)
        {
            if (options is null)
                return default;

            return new CreateContainerParameters
            {
                Env = options.Environment,
                ExposedPorts = options.PortsOptions?.Ports?.ToDictionary(p => p.InternalPort.ToString(CultureInfoSingleton.GetInstance()), p => default(EmptyStruct)),
                HostConfig = new HostConfig
                {
                    PortBindings = options.PortsOptions?.Ports?.ToDictionary(p => p.InternalPort.ToString(CultureInfoSingleton.GetInstance()),
                        p => (IList<PortBinding>)new List<PortBinding>
                        {
                            new PortBinding { HostPort = p.ExposedPort.ToString(CultureInfoSingleton.GetInstance()) }
                        }),
                    PublishAllPorts = options.PortsOptions?.PublishAllPorts ?? true,
                    Mounts = options.Mounts?.Select(m => new Mount
                    {
                        Type = "bind",
                        ReadOnly = m.ReadOnly,
                        Source = m.SourcePath,
                        Target = m.TargetPath
                    }).ToList()
                },
            };
        }
    }
}