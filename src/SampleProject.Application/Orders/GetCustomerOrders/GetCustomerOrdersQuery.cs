namespace SampleProject.Application.Orders.GetCustomerOrders
{
    using System;
    using System.Collections.Generic;
    using Configuration.Queries;

    public class GetCustomerOrdersQuery : IQuery<List<OrderDto>>
    {
        public GetCustomerOrdersQuery(Guid customerId)
        {
            CustomerId = customerId;
        }

        public Guid CustomerId { get; }
    }
}