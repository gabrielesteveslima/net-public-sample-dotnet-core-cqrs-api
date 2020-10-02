namespace SampleProject.Infrastructure.Processing
{
    using System.Threading.Tasks;
    using Application.Configuration.Commands;
    using Autofac;
    using MediatR;

    public static class CommandsExecutor
    {
        public static async Task Execute(ICommand command)
        {
            using (ILifetimeScope scope = CompositionRoot.BeginLifetimeScope())
            {
                IMediator mediator = scope.Resolve<IMediator>();
                await mediator.Send(command);
            }
        }

        public static async Task<TResult> Execute<TResult>(ICommand<TResult> command)
        {
            using (ILifetimeScope scope = CompositionRoot.BeginLifetimeScope())
            {
                IMediator mediator = scope.Resolve<IMediator>();
                return await mediator.Send(command);
            }
        }
    }
}