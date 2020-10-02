namespace SampleProject.Domain.Customers.Orders.Events
{
    using SeedWork;

    public class OrderChangedEvent : DomainEventBase
    {
        public OrderChangedEvent(OrderId orderId)
        {
            OrderId = orderId;
        }

        public OrderId OrderId { get; }
    }
}