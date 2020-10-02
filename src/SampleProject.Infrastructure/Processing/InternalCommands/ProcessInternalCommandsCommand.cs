namespace SampleProject.Infrastructure.Processing.InternalCommands
{
    using Application.Configuration.Commands;
    using MediatR;
    using Outbox;

    internal class ProcessInternalCommandsCommand : CommandBase<Unit>, IRecurringCommand
    {
    }
}