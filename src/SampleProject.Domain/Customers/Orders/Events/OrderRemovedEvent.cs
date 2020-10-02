namespace SampleProject.Domain.Customers.Orders.Events
{
    using SeedWork;

    public class OrderRemovedEvent : DomainEventBase
    {
        public OrderRemovedEvent(OrderId orderId)
        {
            OrderId = orderId;
        }

        public OrderId OrderId { get; }
    }
}