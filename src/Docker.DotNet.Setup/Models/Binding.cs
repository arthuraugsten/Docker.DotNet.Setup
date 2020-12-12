namespace Docker.DotNet.Setup.Models
{
    public sealed class Binding
    {
        public Binding(int internalPort)
            : this(internalPort, internalPort) { }

        public Binding(int internalPort, int exposedPort)
        {
            InternalPort = internalPort;
            ExposedPort = exposedPort;
        }

        public int InternalPort { get; }
        public int ExposedPort { get; }
    }
}