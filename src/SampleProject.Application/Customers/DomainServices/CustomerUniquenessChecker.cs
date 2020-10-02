namespace SampleProject.Application.Customers.DomainServices
{
    using System.Data;
    using Configuration.Data;
    using Dapper;
    using Domain.Customers;

    public class CustomerUniquenessChecker : ICustomerUniquenessChecker
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public CustomerUniquenessChecker(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public bool IsUnique(string customerEmail)
        {
            IDbConnection connection = _sqlConnectionFactory.GetOpenConnection();

            const string sql = "SELECT TOP 1 1" +
                               "FROM [orders].[Customers] AS [Customer] " +
                               "WHERE [Customer].[Email] = @Email";
            int? customersNumber = connection.QuerySingleOrDefault<int?>(sql,
                new {Email = customerEmail});

            return !customersNumber.HasValue;
        }
    }
}