using Docker.DotNet.Setup.Exceptions;
using System;
using Xunit;

namespace Docker.DotNet.Setup.UnitTests.Exceptions
{
    public sealed class UnsupportedPlataformExceptionTests
    {
        private const string DefaultMessage = "Unsupported operational system.";
        private const string CustomMessage = "Custom exception message";

        private UnsupportedPlataformException _exception;

        [Fact]
        public void ShouldCreateExceptionWithDefaultMessage()
        {
            _exception = new UnsupportedPlataformException();
            Assert.Equal(DefaultMessage, _exception.Message);
            Assert.Null(_exception.InnerException);
        }

        [Fact]
        public void ShouldCreateExceptionWithCustomMessage()
        {
            _exception = new UnsupportedPlataformException(CustomMessage);
            Assert.Equal(CustomMessage, _exception.Message);
            Assert.Null(_exception.InnerException);
        }

        [Fact]
        public void ShouldCreateExceptionWithCustomMessageAndInnerException()
        {
            _exception = new UnsupportedPlataformException(CustomMessage, new ArgumentNullException());
            Assert.Equal(CustomMessage, _exception.Message);
            Assert.NotNull(_exception.InnerException);
            Assert.IsType<ArgumentNullException>(_exception.InnerException);
        }
    }
}