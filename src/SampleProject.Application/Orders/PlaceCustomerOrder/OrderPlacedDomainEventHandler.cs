namespace SampleProject.Application.Orders.PlaceCustomerOrder
{
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Customers.Orders.Events;
    using Domain.Payments;
    using MediatR;

    public class OrderPlacedDomainEventHandler : INotificationHandler<OrderPlacedEvent>
    {
        private readonly IPaymentRepository _paymentRepository;

        public OrderPlacedDomainEventHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task Handle(OrderPlacedEvent notification, CancellationToken cancellationToken)
        {
            Payment newPayment = new Payment(notification.OrderId);

            await _paymentRepository.AddAsync(newPayment);
        }
    }
}