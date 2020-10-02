namespace SampleProject.Application.Configuration.DomainEvents
{
    using System;
    using Domain.SeedWork;
    using Newtonsoft.Json;

    public class DomainNotificationBase<T> : IDomainEventNotification<T> where T : IDomainEvent
    {
        public DomainNotificationBase(T domainEvent)
        {
            Id = Guid.NewGuid();
            DomainEvent = domainEvent;
        }

        [JsonIgnore] public T DomainEvent { get; }

        public Guid Id { get; }
    }
}