namespace SampleProject.Domain.Payments
{
    using Customers.Orders;
    using SeedWork;

    public class PaymentCreatedEvent : DomainEventBase
    {
        public PaymentCreatedEvent(PaymentId paymentId, OrderId orderId)
        {
            PaymentId = paymentId;
            OrderId = orderId;
        }

        public PaymentId PaymentId { get; }

        public OrderId OrderId { get; }
    }
}