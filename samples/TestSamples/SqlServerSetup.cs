using Docker.DotNet.Setup;
using Docker.DotNet.Setup.Models;
using Microsoft.Data.SqlClient;
using Polly;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace TestSamples
{
    public sealed class SqlServerSetup : ContainerSetup
    {
        private const string _password = "4Jgz2HmDKS";

        protected override Task WaitContainerAsync()
        {
            const int attemps = 20;

            var politica = Policy
                .HandleResult(false)
                .Or<SqlException>()
                .Or<IOException>()
                .WaitAndRetry(
                    retryCount: attemps,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(6),
                    onRetry: (_, __, attemp, ___) => Console.WriteLine($"Waiting service {nameof(SqlServerSetup)} => Attempt {attemp} of {attemps}")
                );

            using var connection = new SqlConnection($"Server=tcp:127.0.0.1,5433;Database=master;User Id=sa;Password={_password};");

            politica.Execute(() =>
            {
                if (connection.State != ConnectionState.Closed)
                    return true;

                connection.Open();

                using var command = connection.CreateCommand();
                command.CommandText = "SELECT 1";

                return command.ExecuteNonQuery() == 1;
            });

            connection.Close();

            return Task.CompletedTask;
        }

        protected override ContainerOptions Options { get; } = new ContainerOptions
        {
            Name = "sqlserver",
            ImageName = "mcr.microsoft.com/mssql/server",
            ImageTag = "2019-GA-ubuntu-16.04",
            Environment = new List<string>(3)
            {
                "ACCEPT_EULA=Y",
                "SA_PASSWORD=4Jgz2HmDKS",
                "MSSQL_PID=Developer"
            },
            PortsOptions = new PortOptions(new[] { new Binding(1433, 5433) })
        };
    }
}