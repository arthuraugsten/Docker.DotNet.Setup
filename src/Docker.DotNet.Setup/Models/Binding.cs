namespace Docker.DotNet.Setup.Models
{
    public sealed class Binding
    {
        private int _exposedPort;

        public int InternalPort { get; set; }
        public int ExposedPort { get => _exposedPort == default ? InternalPort : _exposedPort; set => _exposedPort = value; }
    }
}