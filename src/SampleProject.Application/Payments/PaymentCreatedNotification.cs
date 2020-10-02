namespace SampleProject.Application.Payments
{
    using Configuration.DomainEvents;
    using Domain.Payments;
    using Newtonsoft.Json;

    public class PaymentCreatedNotification : DomainNotificationBase<PaymentCreatedEvent>
    {
        public PaymentCreatedNotification(PaymentCreatedEvent domainEvent) : base(domainEvent)
        {
            PaymentId = domainEvent.PaymentId;
        }

        [JsonConstructor]
        public PaymentCreatedNotification(PaymentId paymentId) : base(null)
        {
            PaymentId = paymentId;
        }

        public PaymentId PaymentId { get; }
    }
}