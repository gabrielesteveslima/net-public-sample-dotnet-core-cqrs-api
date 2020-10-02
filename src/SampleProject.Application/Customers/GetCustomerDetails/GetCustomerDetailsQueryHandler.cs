namespace SampleProject.Application.Customers.GetCustomerDetails
{
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;
    using Configuration.Data;
    using Configuration.Queries;
    using Dapper;

    public class GetCustomerDetailsQueryHandler : IQueryHandler<GetCustomerDetailsQuery, CustomerDetailsDto>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetCustomerDetailsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public Task<CustomerDetailsDto> Handle(GetCustomerDetailsQuery request, CancellationToken cancellationToken)
        {
            const string sql = "SELECT " +
                               "[Customer].[Id], " +
                               "[Customer].[Name], " +
                               "[Customer].[Email], " +
                               "[Customer].[WelcomeEmailWasSent] " +
                               "FROM orders.v_Customers AS [Customer] " +
                               "WHERE [Customer].[Id] = @CustomerId ";

            IDbConnection connection = _sqlConnectionFactory.GetOpenConnection();

            return connection.QuerySingleAsync<CustomerDetailsDto>(sql, new {request.CustomerId});
        }
    }
}