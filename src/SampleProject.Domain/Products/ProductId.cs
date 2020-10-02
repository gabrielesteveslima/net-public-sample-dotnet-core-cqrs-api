namespace SampleProject.Domain.Products
{
    using System;
    using SeedWork;

    public class ProductId : TypedIdValueBase
    {
        public ProductId(Guid value) : base(value)
        {
        }
    }
}