namespace SampleProject.Application.Customers.IntegrationHandlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Configuration.Commands;
    using Domain.Customers;
    using Domain.Customers.Orders;
    using MediatR;

    public class MarkCustomerAsWelcomedCommandHandler : ICommandHandler<MarkCustomerAsWelcomedCommand, Unit>
    {
        private readonly ICustomerRepository _customerRepository;

        public MarkCustomerAsWelcomedCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Unit> Handle(MarkCustomerAsWelcomedCommand command, CancellationToken cancellationToken)
        {
            Customer customer = await _customerRepository.GetByIdAsync(command.CustomerId);

            customer.MarkAsWelcomedByEmail();

            return Unit.Value;
        }
    }
}