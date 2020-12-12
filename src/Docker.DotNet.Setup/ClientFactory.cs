using Docker.DotNet.Setup.Abstractions;
using Docker.DotNet.Setup.Exceptions;
using System;
using System.Runtime.InteropServices;

namespace Docker.DotNet.Setup
{
    public sealed class ClientFactory : IClientFactory
    {
        public IClientFacade CreateClient()
        {
            try
            {
                using (var dockerConfiguration = new DockerClientConfiguration(GetDockerApiUri()))
                    return new ClientFacade(dockerConfiguration.CreateClient());
            }
            catch (DockerApiException ex)
            {
                throw new InvalidOperationException("Something was wrong on docker API communication.", ex);
            }
        }

        private Uri GetDockerApiUri()
        {
            const string windowsUri = "npipe://./pipe/docker_engine";
            const string linuxUri = "unix:/var/run/docker.sock";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return new Uri(windowsUri);

            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                ? new Uri(linuxUri)
                : throw new UnsupportedPlataformException();
        }
    }
}