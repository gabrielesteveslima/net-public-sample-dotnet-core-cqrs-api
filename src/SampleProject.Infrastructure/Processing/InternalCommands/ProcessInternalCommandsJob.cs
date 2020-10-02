namespace SampleProject.Infrastructure.Processing.InternalCommands
{
    using System.Threading.Tasks;
    using global::Quartz;

    [DisallowConcurrentExecution]
    public class ProcessInternalCommandsJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await CommandsExecutor.Execute(new ProcessInternalCommandsCommand());
        }
    }
}