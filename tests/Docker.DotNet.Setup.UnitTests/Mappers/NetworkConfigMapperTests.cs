using Docker.DotNet.Setup.Abstractions;
using Docker.DotNet.Setup.Mappers;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Docker.DotNet.Setup.UnitTests.Mappers
{
    public sealed class NetworkConfigMapperTests
    {
        private const string ExceptionMessage = "Value cannot be null. (Parameter '{0}')";

        private readonly INetworkSetup _networkSetup = new NetworkSetupMock();

        [Fact]
        public void ShouldThrowsExceptionWhenSetupIsDefault()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => NetworkConfigMapper.MapFrom(default, string.Empty));
            Assert.Equal(string.Format(ExceptionMessage, "setup"), exception.Message);
        }

        [Theory]
        [InlineData(default(string))]
        [InlineData("")]
        public void ShouldThrowsExceptionWhenNetworkIdIsDefaultOrEmpty(string networkId)
        {
            var exception = Assert.Throws<ArgumentNullException>(() => NetworkConfigMapper.MapFrom(_networkSetup, networkId));
            Assert.Equal(string.Format(ExceptionMessage, nameof(networkId)), exception.Message);
        }

        [Fact]
        public void ShouldReturnMappedConfigs()
        {
            const string networkId = "network";

            var configs = NetworkConfigMapper.MapFrom(_networkSetup, networkId);

            Assert.Equal(1, configs.EndpointsConfig.Count);

            var currentConfig = configs.EndpointsConfig.FirstOrDefault();

            Assert.Equal(_networkSetup.Name, currentConfig.Key);
            Assert.Equal(networkId, currentConfig.Value.NetworkID);
        }

        private class NetworkSetupMock : INetworkSetup
        {
            public string Name => "UnitTest";
            public bool ShouldRemoveNetworkOnExit => default;

            public Task CreateNetworkAsync() => Task.CompletedTask;
        }
    }
}