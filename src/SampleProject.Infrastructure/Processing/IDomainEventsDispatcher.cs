namespace SampleProject.Infrastructure.Processing
{
    using System.Threading.Tasks;

    public interface IDomainEventsDispatcher
    {
        Task DispatchEventsAsync();
    }
}