namespace Docker.DotNet.Setup.Models
{
    public sealed class ContainerInfo
    {
        private const string RunningState = "running";

        public ContainerInfo(string id, string status)
        {
            Id = id;
            State = status;
        }

        public string Id { get; }
        public string State { get; }

        public bool IsRunning => State == RunningState;
    }
}