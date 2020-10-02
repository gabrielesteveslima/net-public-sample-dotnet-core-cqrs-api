namespace SampleProject.Domain.SeedWork
{
    using System;
    using MediatR;

    public interface IDomainEvent : INotification
    {
        DateTime OccurredOn { get; }
    }
}