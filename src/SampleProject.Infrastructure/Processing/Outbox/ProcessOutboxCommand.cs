namespace SampleProject.Infrastructure.Processing.Outbox
{
    using Application.Configuration.Commands;
    using MediatR;

    public class ProcessOutboxCommand : CommandBase<Unit>, IRecurringCommand
    {
    }
}