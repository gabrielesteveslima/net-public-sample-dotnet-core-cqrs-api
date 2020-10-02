namespace SampleProject.Domain.Customers
{
    using SeedWork;

    public class CustomerRegisteredEvent : DomainEventBase
    {
        public CustomerRegisteredEvent(CustomerId customerId)
        {
            CustomerId = customerId;
        }

        public CustomerId CustomerId { get; }
    }
}