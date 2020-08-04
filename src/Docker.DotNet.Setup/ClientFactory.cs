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
            if (_plataformInfo.IsWindows)
                return new Uri("npipe://./pipe/docker_engine");

            if (_plataformInfo.IsLinux)
                return new Uri("unix:/var/run/docker.sock");

            throw new UnsupportedPlataformException();
        }
    }
}