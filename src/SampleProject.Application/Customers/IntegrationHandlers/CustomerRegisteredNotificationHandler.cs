namespace SampleProject.Application.Customers.IntegrationHandlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Configuration.Processing;
    using MediatR;

    public class CustomerRegisteredNotificationHandler : INotificationHandler<CustomerRegisteredNotification>
    {
        private readonly ICommandsScheduler _commandsScheduler;

        public CustomerRegisteredNotificationHandler(
            ICommandsScheduler commandsScheduler)
        {
            _commandsScheduler = commandsScheduler;
        }

        public async Task Handle(CustomerRegisteredNotification notification, CancellationToken cancellationToken)
        {
            // Send welcome e-mail message...

            await _commandsScheduler.EnqueueAsync(new MarkCustomerAsWelcomedCommand(
                Guid.NewGuid(),
                notification.CustomerId));
        }
    }
}