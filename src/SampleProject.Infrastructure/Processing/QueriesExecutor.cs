namespace SampleProject.Infrastructure.Processing
{
    using System.Threading.Tasks;
    using Application.Configuration.Queries;
    using Autofac;
    using MediatR;

    public static class QueriesExecutor
    {
        public static async Task<TResult> Execute<TResult>(IQuery<TResult> query)
        {
            using (ILifetimeScope scope = CompositionRoot.BeginLifetimeScope())
            {
                IMediator mediator = scope.Resolve<IMediator>();

                return await mediator.Send(query);
            }
        }
    }
}