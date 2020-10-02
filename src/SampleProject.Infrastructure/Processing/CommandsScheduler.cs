namespace SampleProject.Infrastructure.Processing
{
    using System;
    using System.Data;
    using System.Threading.Tasks;
    using Application.Configuration.Commands;
    using Application.Configuration.Data;
    using Application.Configuration.Processing;
    using Dapper;
    using Newtonsoft.Json;

    public class CommandsScheduler : ICommandsScheduler
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public CommandsScheduler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task EnqueueAsync<T>(ICommand<T> command)
        {
            IDbConnection connection = _sqlConnectionFactory.GetOpenConnection();

            const string sqlInsert =
                "INSERT INTO [app].[InternalCommands] ([Id], [EnqueueDate] , [Type], [Data]) VALUES " +
                "(@Id, @EnqueueDate, @Type, @Data)";

            await connection.ExecuteAsync(sqlInsert,
                new
                {
                    command.Id,
                    EnqueueDate = DateTime.UtcNow,
                    Type = command.GetType().FullName,
                    Data = JsonConvert.SerializeObject(command)
                });
        }
    }
}