namespace SampleProject.Application.Orders.RemoveCustomerOrder
{
    using System.Threading;
    using System.Threading.Tasks;
    using Configuration.Commands;
    using Domain.Customers;
    using Domain.Customers.Orders;
    using MediatR;

    public class RemoveCustomerOrderCommandHandler : ICommandHandler<RemoveCustomerOrderCommand>
    {
        private readonly ICustomerRepository _customerRepository;

        public RemoveCustomerOrderCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Unit> Handle(RemoveCustomerOrderCommand request, CancellationToken cancellationToken)
        {
            Customer customer = await _customerRepository.GetByIdAsync(new CustomerId(request.CustomerId));

            customer.RemoveOrder(new OrderId(request.OrderId));

            return Unit.Value;
        }
    }
}