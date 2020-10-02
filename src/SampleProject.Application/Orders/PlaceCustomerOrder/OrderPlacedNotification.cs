namespace SampleProject.Application.Orders.PlaceCustomerOrder
{
    using Configuration.DomainEvents;
    using Domain.Customers;
    using Domain.Customers.Orders;
    using Domain.Customers.Orders.Events;
    using Newtonsoft.Json;

    public class OrderPlacedNotification : DomainNotificationBase<OrderPlacedEvent>
    {
        public OrderPlacedNotification(OrderPlacedEvent domainEvent) : base(domainEvent)
        {
            OrderId = domainEvent.OrderId;
            CustomerId = domainEvent.CustomerId;
        }

        [JsonConstructor]
        public OrderPlacedNotification(OrderId orderId, CustomerId customerId) : base(null)
        {
            OrderId = orderId;
            CustomerId = customerId;
        }

        public OrderId OrderId { get; }
        public CustomerId CustomerId { get; }
    }
}