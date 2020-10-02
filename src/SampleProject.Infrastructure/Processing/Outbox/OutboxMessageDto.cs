namespace SampleProject.Infrastructure.Processing.Outbox
{
    using System;

    public class OutboxMessageDto
    {
        public Guid Id { get; set; }

        public string Type { get; set; }

        public string Data { get; set; }
    }
}