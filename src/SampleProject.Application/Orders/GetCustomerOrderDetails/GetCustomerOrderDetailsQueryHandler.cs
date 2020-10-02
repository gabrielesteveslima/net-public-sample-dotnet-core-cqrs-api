﻿namespace SampleProject.Application.Orders.GetCustomerOrderDetails
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;
    using Configuration.Data;
    using Configuration.Queries;
    using Dapper;

    internal sealed class
        GetCustomerOrderDetailsQueryHandler : IQueryHandler<GetCustomerOrderDetailsQuery, OrderDetailsDto>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        internal GetCustomerOrderDetailsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<OrderDetailsDto> Handle(GetCustomerOrderDetailsQuery request,
            CancellationToken cancellationToken)
        {
            IDbConnection connection = _sqlConnectionFactory.GetOpenConnection();

            const string sql = "SELECT " +
                               "[Order].[Id], " +
                               "[Order].[IsRemoved], " +
                               "[Order].[Value], " +
                               "[Order].[Currency] " +
                               "FROM orders.v_Orders AS [Order] " +
                               "WHERE [Order].Id = @OrderId";
            OrderDetailsDto order =
                await connection.QuerySingleOrDefaultAsync<OrderDetailsDto>(sql, new {request.OrderId});

            const string sqlProducts = "SELECT " +
                                       "[Order].[ProductId] AS [Id], " +
                                       "[Order].[Quantity], " +
                                       "[Order].[Name], " +
                                       "[Order].[Value], " +
                                       "[Order].[Currency] " +
                                       "FROM orders.v_OrderProducts AS [Order] " +
                                       "WHERE [Order].OrderId = @OrderId";
            IEnumerable<ProductDto> products =
                await connection.QueryAsync<ProductDto>(sqlProducts, new {request.OrderId});

            order.Products = products.AsList();

            return order;
        }
    }
}