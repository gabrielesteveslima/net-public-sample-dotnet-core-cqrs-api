namespace SampleProject.Application.Orders.GetCustomerOrders
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;
    using Configuration.Data;
    using Configuration.Queries;
    using Dapper;

    internal sealed class GetCustomerOrdersQueryHandler : IQueryHandler<GetCustomerOrdersQuery, List<OrderDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        internal GetCustomerOrdersQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<List<OrderDto>> Handle(GetCustomerOrdersQuery request, CancellationToken cancellationToken)
        {
            IDbConnection connection = _sqlConnectionFactory.GetOpenConnection();
            const string sql = "SELECT " +
                               "[Order].[Id], " +
                               "[Order].[IsRemoved], " +
                               "[Order].[Value], " +
                               "[Order].[Currency] " +
                               "FROM orders.v_Orders AS [Order] " +
                               "WHERE [Order].CustomerId = @CustomerId";
            IEnumerable<OrderDto> orders = await connection.QueryAsync<OrderDto>(sql, new {request.CustomerId});

            return orders.AsList();
        }
    }
}