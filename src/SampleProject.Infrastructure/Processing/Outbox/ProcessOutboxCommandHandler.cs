namespace SampleProject.Infrastructure.Processing.Outbox
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Configuration.Commands;
    using Application.Configuration.Data;
    using Application.Configuration.DomainEvents;
    using Dapper;
    using MediatR;
    using Newtonsoft.Json;
    using Serilog.Context;
    using Serilog.Core;
    using Serilog.Events;

    internal class ProcessOutboxCommandHandler : ICommandHandler<ProcessOutboxCommand, Unit>
    {
        private readonly IMediator _mediator;

        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public ProcessOutboxCommandHandler(IMediator mediator, ISqlConnectionFactory sqlConnectionFactory)
        {
            _mediator = mediator;
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(ProcessOutboxCommand command, CancellationToken cancellationToken)
        {
            IDbConnection connection = _sqlConnectionFactory.GetOpenConnection();
            const string sql = "SELECT " +
                               "[OutboxMessage].[Id], " +
                               "[OutboxMessage].[Type], " +
                               "[OutboxMessage].[Data] " +
                               "FROM [app].[OutboxMessages] AS [OutboxMessage] " +
                               "WHERE [OutboxMessage].[ProcessedDate] IS NULL";

            IEnumerable<OutboxMessageDto> messages = await connection.QueryAsync<OutboxMessageDto>(sql);
            List<OutboxMessageDto> messagesList = messages.AsList();

            const string sqlUpdateProcessedDate = "UPDATE [app].[OutboxMessages] " +
                                                  "SET [ProcessedDate] = @Date " +
                                                  "WHERE [Id] = @Id";
            if (messagesList.Count > 0)
            {
                foreach (OutboxMessageDto message in messagesList)
                {
                    Type? type = Assemblies.Application
                        .GetType(message.Type);
                    IDomainEventNotification request =
                        JsonConvert.DeserializeObject(message.Data, type) as IDomainEventNotification;

                    using (LogContext.Push(new OutboxMessageContextEnricher(request)))
                    {
                        await _mediator.Publish(request, cancellationToken);

                        await connection.ExecuteAsync(sqlUpdateProcessedDate, new {Date = DateTime.UtcNow, message.Id});
                    }
                }
            }

            return Unit.Value;
        }

        private class OutboxMessageContextEnricher : ILogEventEnricher
        {
            private readonly IDomainEventNotification _notification;

            public OutboxMessageContextEnricher(IDomainEventNotification notification)
            {
                _notification = notification;
            }

            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                logEvent.AddOrUpdateProperty(new LogEventProperty("Context",
                    new ScalarValue($"OutboxMessage:{_notification.Id.ToString()}")));
            }
        }
    }
}