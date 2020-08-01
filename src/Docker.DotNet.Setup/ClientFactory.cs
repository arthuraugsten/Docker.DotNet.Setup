using Docker.DotNet.Setup.Abstractions;
using Docker.DotNet.Setup.Exceptions;
using System;

namespace Docker.DotNet.Setup
{
    public static class ClientFactory
    {
        private static readonly IPlataformInfo _operationalSystemInfo = new PlataformIndo();

        public static DockerClient CreateClient()
        {
            using (var dockerConfiguration = new DockerClientConfiguration(GetDockerApiUri()))
                return dockerConfiguration.CreateClient();
        }

        private static Uri GetDockerApiUri()
        {
            if (_operationalSystemInfo.IsWindows)
                return new Uri("npipe://./pipe/docker_engine");

            if (_operationalSystemInfo.IsLinux)
                return new Uri("unix:/var/run/docker.sock");

            throw new UnsupportedPlataform();
        }
    }
}