namespace SampleProject.Domain.SeedWork
{
    public interface IBusinessRule
    {
        string Message { get; }
        bool IsBroken();
    }
}