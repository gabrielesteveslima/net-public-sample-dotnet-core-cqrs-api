namespace SampleProject.Infrastructure.Processing.InternalCommands
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Configuration.Commands;
    using Application.Configuration.Data;
    using Dapper;
    using MediatR;
    using Newtonsoft.Json;

    internal class ProcessInternalCommandsCommandHandler : ICommandHandler<ProcessInternalCommandsCommand, Unit>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public ProcessInternalCommandsCommandHandler(
            ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(ProcessInternalCommandsCommand command, CancellationToken cancellationToken)
        {
            IDbConnection connection = _sqlConnectionFactory.GetOpenConnection();

            const string sql = "SELECT " +
                               "[Command].[Type], " +
                               "[Command].[Data] " +
                               "FROM [app].[InternalCommands] AS [Command] " +
                               "WHERE [Command].[ProcessedDate] IS NULL";
            IEnumerable<InternalCommandDto> commands = await connection.QueryAsync<InternalCommandDto>(sql);

            List<InternalCommandDto> internalCommandsList = commands.AsList();

            foreach (InternalCommandDto internalCommand in internalCommandsList)
            {
                Type? type = Assemblies.Application.GetType(internalCommand.Type);
                dynamic commandToProcess = JsonConvert.DeserializeObject(internalCommand.Data, type);

                await CommandsExecutor.Execute(commandToProcess);
            }

            return Unit.Value;
        }

        private class InternalCommandDto
        {
            public string Type { get; set; }

            public string Data { get; set; }
        }
    }
}