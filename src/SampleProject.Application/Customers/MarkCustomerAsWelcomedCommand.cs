namespace SampleProject.Application.Customers
{
    using System;
    using Configuration.Commands;
    using Domain.Customers;
    using MediatR;
    using Newtonsoft.Json;

    public class MarkCustomerAsWelcomedCommand : InternalCommandBase<Unit>
    {
        [JsonConstructor]
        public MarkCustomerAsWelcomedCommand(Guid id, CustomerId customerId) : base(id)
        {
            CustomerId = customerId;
        }

        public CustomerId CustomerId { get; }
    }
}