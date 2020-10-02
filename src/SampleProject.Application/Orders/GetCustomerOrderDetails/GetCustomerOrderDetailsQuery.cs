namespace SampleProject.Application.Orders.GetCustomerOrderDetails
{
    using System;
    using Configuration.Queries;

    public class GetCustomerOrderDetailsQuery : IQuery<OrderDetailsDto>
    {
        public GetCustomerOrderDetailsQuery(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; }
    }
}