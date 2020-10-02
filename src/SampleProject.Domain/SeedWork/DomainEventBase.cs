namespace SampleProject.Domain.SeedWork
{
    using System;

    public class DomainEventBase : IDomainEvent
    {
        public DomainEventBase()
        {
            OccurredOn = DateTime.Now;
        }

        public DateTime OccurredOn { get; }
    }
}