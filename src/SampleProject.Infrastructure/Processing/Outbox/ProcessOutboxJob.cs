namespace SampleProject.Infrastructure.Processing.Outbox
{
    using System.Threading.Tasks;
    using global::Quartz;

    [DisallowConcurrentExecution]
    public class ProcessOutboxJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await CommandsExecutor.Execute(new ProcessOutboxCommand());
        }
    }
}