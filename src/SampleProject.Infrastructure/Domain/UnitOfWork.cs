namespace SampleProject.Infrastructure.Domain
{
    using System.Threading;
    using System.Threading.Tasks;
    using Database;
    using Processing;
    using SampleProject.Domain.SeedWork;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDomainEventsDispatcher _domainEventsDispatcher;
        private readonly OrdersContext _ordersContext;

        public UnitOfWork(
            OrdersContext ordersContext,
            IDomainEventsDispatcher domainEventsDispatcher)
        {
            _ordersContext = ordersContext;
            _domainEventsDispatcher = domainEventsDispatcher;
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            await _domainEventsDispatcher.DispatchEventsAsync();
            return await _ordersContext.SaveChangesAsync(cancellationToken);
        }
    }
}