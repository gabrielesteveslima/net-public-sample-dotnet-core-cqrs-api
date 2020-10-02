namespace SampleProject.Infrastructure.Processing
{
    using System;
    using System.Threading.Tasks;

    public interface ICommandsDispatcher
    {
        Task DispatchCommandAsync(Guid id);
    }
}