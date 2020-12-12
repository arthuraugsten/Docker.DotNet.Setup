using Docker.DotNet.Setup.Abstractions;
using Docker.DotNet.Setup.Exceptions;
using System;

namespace Docker.DotNet.Setup
{
    public sealed class ClientFactory : IClientFactory
    {
        private readonly IPlataformInfo _plataformInfo;

        public ClientFactory(IPlataformInfo plataformInfo)
            => _plataformInfo = plataformInfo;

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

            if (_plataformInfo.IsWindows)
                return new Uri(windowsUri);

            return _plataformInfo.IsLinux ? new Uri(linuxUri) : throw new UnsupportedPlataformException();
        }
    }
}