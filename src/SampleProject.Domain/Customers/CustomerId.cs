namespace SampleProject.Domain.Customers
{
    using System;
    using SeedWork;

    public class CustomerId : TypedIdValueBase
    {
        public CustomerId(Guid value) : base(value)
        {
        }
    }
}