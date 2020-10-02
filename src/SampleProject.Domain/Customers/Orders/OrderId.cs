namespace SampleProject.Domain.Customers.Orders
{
    using System;
    using SeedWork;

    public class OrderId : TypedIdValueBase
    {
        public OrderId(Guid value) : base(value)
        {
        }
    }
}