namespace SampleProject.IntegrationTests.SeedWork
{
    using System;
    using Application.Configuration;

    public class ExecutionContextMock : IExecutionContextAccessor
    {
        public Guid CorrelationId { get; set; }

        public bool IsAvailable { get; set; }
    }
}