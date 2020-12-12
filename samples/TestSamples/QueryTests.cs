using Microsoft.Data.SqlClient;
using Xunit;

namespace TestSamples
{
    public sealed class QueryTests : TestFixture
    {
        [Fact]
        public void Should_connect_on_sql_server()
        {
            using var connection = new SqlConnection($"Server=tcp:127.0.0.1,5433;Database=master;User Id=sa;Password=4Jgz2HmDKS;");

            try
            {
                connection.Open();

                using var command = connection.CreateCommand();
                command.CommandText = "SELECT 1";

                Assert.NotEqual(1, command.ExecuteNonQuery());
            }
            finally
            {
                connection.Close();
            }
        }
    }
}