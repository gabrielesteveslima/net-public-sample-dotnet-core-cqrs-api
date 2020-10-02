namespace SampleProject.Domain.Products
{
    using SeedWork;
    using SharedKernel;

    public class ProductPriceData : ValueObject
    {
        public ProductPriceData(ProductId productId, MoneyValue price)
        {
            ProductId = productId;
            Price = price;
        }

        public ProductId ProductId { get; }

        public MoneyValue Price { get; }
    }
}