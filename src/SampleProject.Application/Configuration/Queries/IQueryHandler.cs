namespace SampleProject.Application.Configuration.Queries
{
    using MediatR;

    public interface IQueryHandler<in TQuery, TResult> :
        IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
    }
}