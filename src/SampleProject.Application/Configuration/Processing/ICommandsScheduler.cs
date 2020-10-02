namespace SampleProject.Application.Configuration.Processing
{
    using System.Threading.Tasks;
    using Commands;

    public interface ICommandsScheduler
    {
        Task EnqueueAsync<T>(ICommand<T> command);
    }
}