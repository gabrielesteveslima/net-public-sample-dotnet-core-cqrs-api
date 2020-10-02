namespace SampleProject.Domain.Customers.Orders.Events
{
    using SeedWork;
    using SharedKernel;

    public class OrderPlacedEvent : DomainEventBase
    {
        public OrderPlacedEvent(
            OrderId orderId,
            CustomerId customerId,
            MoneyValue value)
        {
            OrderId = orderId;
            CustomerId = customerId;
            Value = value;
        }

        public OrderId OrderId { get; }

        public CustomerId CustomerId { get; }

        public MoneyValue Value { get; }
    }
}