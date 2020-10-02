namespace SampleProject.Application.Orders
{
    using System;

    public class ProductDto
    {
        public ProductDto()
        {
        }

        public ProductDto(Guid id, int quantity)
        {
            Id = id;
            Quantity = quantity;
        }

        public Guid Id { get; set; }

        public int Quantity { get; set; }


        public string Name { get; set; }
    }
}