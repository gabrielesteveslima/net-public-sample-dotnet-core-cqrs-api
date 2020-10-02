namespace SampleProject.Domain.Products
{
    using System.Collections.Generic;
    using SeedWork;

    public class Product : Entity, IAggregateRoot
    {
        private List<ProductPrice> _prices;

        private Product()
        {
        }

        public ProductId Id { get; private set; }

        public string Name { get; private set; }
    }
}