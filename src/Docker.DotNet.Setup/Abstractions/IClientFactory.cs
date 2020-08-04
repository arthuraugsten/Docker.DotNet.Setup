namespace Docker.DotNet.Setup.Abstractions
{
    public interface IClientFactory
    {
        IClientFacade CreateClient();
    }
}