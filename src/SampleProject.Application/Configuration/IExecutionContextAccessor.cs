namespace SampleProject.Application.Configuration
{
    using System;

    public interface IExecutionContextAccessor
    {
        Guid CorrelationId { get; }

        bool IsAvailable { get; }
    }
}