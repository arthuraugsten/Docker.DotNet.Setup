namespace Docker.DotNet.Setup.Models
{
    public sealed class ContainerInfo
    {
        public ContainerInfo(string id, string status)
        {
            Id = id;
            State = status;
        }

        public string Id { get; }
        public string State { get; }

        public void Deconstruct(out string id, out string state)
        {
            id = Id;
            state = State;
        }
    }
}