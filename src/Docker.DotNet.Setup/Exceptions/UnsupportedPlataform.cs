using System;

namespace Docker.DotNet.Setup.Exceptions
{
    public sealed class UnsupportedPlataform : Exception
    {
        private const string _message = "Unsupported operational system.";

        public UnsupportedPlataform() : base(_message)
        { }

        public UnsupportedPlataform(string message) : base(message)
        { }

        public UnsupportedPlataform(string message, Exception innerException) : base(message, innerException)
        { }
    }
}