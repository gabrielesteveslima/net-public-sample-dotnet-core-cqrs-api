namespace SampleProject.Application.Customers.IntegrationHandlers
{
    using Configuration.DomainEvents;
    using Domain.Customers;
    using Newtonsoft.Json;

    public class CustomerRegisteredNotification : DomainNotificationBase<CustomerRegisteredEvent>
    {
        public CustomerRegisteredNotification(CustomerRegisteredEvent domainEvent) : base(domainEvent)
        {
            CustomerId = domainEvent.CustomerId;
        }

        [JsonConstructor]
        public CustomerRegisteredNotification(CustomerId customerId) : base(null)
        {
            CustomerId = customerId;
        }

        public CustomerId CustomerId { get; }
    }
}