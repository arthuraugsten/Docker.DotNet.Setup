using Docker.DotNet.Setup;
using System.Threading.Tasks;
using Xunit;

namespace TestSamples
{
    public abstract class TestFixture : IAsyncLifetime
    {
        private readonly ContainerPool _pool;

        public TestFixture()
        {
            var client = new ClientFactory().CreateClient();
            var network = new AppNetworkSetup(client);

            _pool = new ContainerPool(client, network);
            _pool.Add<SqlServerSetup>();
        }

        public async Task InitializeAsync() => await _pool.ConfigureAllAsync();
        public async Task DisposeAsync() => await _pool.DiscardAllAsync();
    }
}