namespace SampleProject.Application.Orders.RemoveCustomerOrder
{
    using System;
    using Configuration.Commands;

    public class RemoveCustomerOrderCommand : CommandBase
    {
        public RemoveCustomerOrderCommand(
            Guid customerId,
            Guid orderId)
        {
            CustomerId = customerId;
            OrderId = orderId;
        }

        public Guid CustomerId { get; }

        public Guid OrderId { get; }
    }
}