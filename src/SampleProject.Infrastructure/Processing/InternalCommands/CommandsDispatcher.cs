namespace SampleProject.Infrastructure.Processing.InternalCommands
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using Application.Customers;
    using Database;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    public class CommandsDispatcher : ICommandsDispatcher
    {
        private readonly IMediator _mediator;
        private readonly OrdersContext _ordersContext;

        public CommandsDispatcher(
            IMediator mediator,
            OrdersContext ordersContext)
        {
            _mediator = mediator;
            _ordersContext = ordersContext;
        }

        public async Task DispatchCommandAsync(Guid id)
        {
            InternalCommand internalCommand =
                await _ordersContext.InternalCommands.SingleOrDefaultAsync(x => x.Id == id);

            Type? type = Assembly.GetAssembly(typeof(MarkCustomerAsWelcomedCommand)).GetType(internalCommand.Type);
            dynamic command = JsonConvert.DeserializeObject(internalCommand.Data, type);

            internalCommand.ProcessedDate = DateTime.UtcNow;

            await _mediator.Send(command);
        }
    }
}