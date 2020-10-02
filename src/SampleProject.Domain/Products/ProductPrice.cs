namespace SampleProject.Domain.Products
{
    using SharedKernel;

    public class ProductPrice
    {
        private ProductPrice()
        {
        }

        public MoneyValue Value { get; private set; }
    }
}