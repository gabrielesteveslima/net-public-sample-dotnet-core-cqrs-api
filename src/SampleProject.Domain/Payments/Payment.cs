namespace SampleProject.Domain.Payments
{
    using System;
    using Customers.Orders;
    using SeedWork;

    public class Payment : Entity, IAggregateRoot
    {
        private readonly OrderId _orderId;
        private DateTime _createDate;

        private bool _emailNotificationIsSent;

        private PaymentStatus _status;

        private Payment()
        {
            // Only for EF.
        }

        public Payment(OrderId orderId)
        {
            Id = new PaymentId(Guid.NewGuid());
            _createDate = DateTime.UtcNow;
            _orderId = orderId;
            _status = PaymentStatus.ToPay;
            _emailNotificationIsSent = false;

            AddDomainEvent(new PaymentCreatedEvent(Id, _orderId));
        }

        public PaymentId Id { get; }

        public void MarkEmailNotificationIsSent()
        {
            _emailNotificationIsSent = true;
        }
    }
}