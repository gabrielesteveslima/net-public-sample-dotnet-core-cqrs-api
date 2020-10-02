namespace SampleProject.Domain.Customers.Orders
{
    using System.Collections.Generic;
    using System.Linq;
    using ForeignExchange;
    using Products;
    using SeedWork;
    using SharedKernel;

    public class OrderProduct : Entity
    {
        private OrderProduct()
        {
        }

        private OrderProduct(
            ProductPriceData productPrice,
            int quantity,
            string currency,
            List<ConversionRate> conversionRates)
        {
            ProductId = productPrice.ProductId;
            Quantity = quantity;

            CalculateValue(productPrice, currency, conversionRates);
        }

        public int Quantity { get; private set; }

        public ProductId ProductId { get; }

        internal MoneyValue Value { get; private set; }

        internal MoneyValue ValueInEUR { get; private set; }

        internal static OrderProduct CreateForProduct(
            ProductPriceData productPrice, int quantity, string currency,
            List<ConversionRate> conversionRates)
        {
            return new OrderProduct(productPrice, quantity, currency, conversionRates);
        }

        internal void ChangeQuantity(ProductPriceData productPrice, int quantity, List<ConversionRate> conversionRates)
        {
            Quantity = quantity;

            CalculateValue(productPrice, Value.Currency, conversionRates);
        }

        private void CalculateValue(ProductPriceData productPrice, string currency,
            List<ConversionRate> conversionRates)
        {
            Value = Quantity * productPrice.Price;
            if (currency == "EUR")
            {
                ValueInEUR = Quantity * productPrice.Price;
            }
            else
            {
                ConversionRate conversionRate =
                    conversionRates.Single(x => x.SourceCurrency == currency && x.TargetCurrency == "EUR");
                ValueInEUR = conversionRate.Convert(Value);
            }
        }
    }
}