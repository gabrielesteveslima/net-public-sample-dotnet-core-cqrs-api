namespace SampleProject.IntegrationTests.SeedWork
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Application.Configuration.Emails;
    using Dapper;
    using Infrastructure;
    using Microsoft.Extensions.DependencyInjection;
    using NSubstitute;
    using NUnit.Framework;
    using Serilog.Core;

    public class TestBase
    {
        protected string ConnectionString;

        protected IEmailSender EmailSender;

        protected EmailsSettings EmailsSettings;

        protected ExecutionContextMock ExecutionContext;

        [SetUp]
        public async Task BeforeEachTest()
        {
            const string connectionStringEnvironmentVariable =
                "ASPNETCORE_SampleProject_IntegrationTests_ConnectionString";
            ConnectionString = Environment.GetEnvironmentVariable(connectionStringEnvironmentVariable);
            if (ConnectionString == null)
            {
                throw new ApplicationException(
                    $"Define connection string to integration tests database using environment variable: {connectionStringEnvironmentVariable}");
            }

            await using SqlConnection sqlConnection = new SqlConnection(ConnectionString);

            await ClearDatabase(sqlConnection);

            EmailsSettings = new EmailsSettings {FromAddressEmail = "from@mail.com"};

            EmailSender = Substitute.For<IEmailSender>();

            ExecutionContext = new ExecutionContextMock();

            ApplicationStartup.Initialize(
                new ServiceCollection(),
                ConnectionString,
                new CacheStore(),
                EmailSender,
                EmailsSettings,
                Logger.None,
                ExecutionContext,
                false);
        }

        private static async Task ClearDatabase(IDbConnection connection)
        {
            const string sql = "DELETE FROM app.InternalCommands " +
                               "DELETE FROM app.OutboxMessages " +
                               "DELETE FROM orders.OrderProducts " +
                               "DELETE FROM orders.Orders " +
                               "DELETE FROM payments.Payments " +
                               "DELETE FROM orders.Customers ";

            await connection.ExecuteScalarAsync(sql);
        }
    }
}