using System;

namespace Docker.DotNet.Setup.Exceptions
{
    public sealed class UnsupportedPlataformException : Exception
    {
        private const string _message = "Unsupported operational system.";

        public UnsupportedPlataformException() : base(_message)
        { }

        public UnsupportedPlataformException(string message)
            : base(message)
        { }

        public UnsupportedPlataformException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}