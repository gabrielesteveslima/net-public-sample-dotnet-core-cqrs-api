namespace SampleProject.Infrastructure.Processing
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Configuration.Commands;
    using Database;
    using InternalCommands;
    using Microsoft.EntityFrameworkCore;
    using SampleProject.Domain.SeedWork;

    public class UnitOfWorkCommandHandlerWithResultDecorator<T, TResult> : ICommandHandler<T, TResult>
        where T : ICommand<TResult>
    {
        private readonly ICommandHandler<T, TResult> _decorated;

        private readonly OrdersContext _ordersContext;

        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkCommandHandlerWithResultDecorator(
            ICommandHandler<T, TResult> decorated,
            IUnitOfWork unitOfWork,
            OrdersContext ordersContext)
        {
            _decorated = decorated;
            _unitOfWork = unitOfWork;
            _ordersContext = ordersContext;
        }

        public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
        {
            TResult result = await _decorated.Handle(command, cancellationToken);

            if (command is InternalCommandBase<TResult>)
            {
                InternalCommand internalCommand =
                    await _ordersContext.InternalCommands.FirstOrDefaultAsync(x => x.Id == command.Id,
                        cancellationToken);

                if (internalCommand != null)
                {
                    internalCommand.ProcessedDate = DateTime.UtcNow;
                }
            }

            await _unitOfWork.CommitAsync(cancellationToken);

            return result;
        }
    }
}