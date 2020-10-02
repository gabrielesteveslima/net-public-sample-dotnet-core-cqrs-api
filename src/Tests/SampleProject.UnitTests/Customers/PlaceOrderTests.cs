namespace SampleProject.UnitTests.Customers
{
    using System;
    using System.Collections.Generic;
    using Domain.Customers;
    using Domain.Customers.Orders;
    using Domain.Customers.Orders.Events;
    using Domain.Customers.Rules;
    using Domain.ForeignExchange;
    using Domain.Products;
    using Domain.SharedKernel;
    using NUnit.Framework;
    using SeedWork;

    [TestFixture]
    public class PlaceOrderTests : TestBase
    {
        [Test]
        public void PlaceOrder_WhenAtLeastOneProductIsAdded_IsSuccessful()
        {
            // Arrange
            Customer customer = CustomerFactory.Create();

            List<OrderProductData> orderProductsData = new List<OrderProductData>();
            orderProductsData.Add(new OrderProductData(SampleProducts.Product1Id, 2));

            List<ProductPriceData> allProductPrices = new List<ProductPriceData>
            {
                SampleProductPrices.Product1EUR, SampleProductPrices.Product1USD
            };

            const string currency = "EUR";
            List<ConversionRate> conversionRates = GetConversionRates();

            // Act
            customer.PlaceOrder(
                orderProductsData,
                allProductPrices,
                currency,
                conversionRates);

            // Assert
            OrderPlacedEvent orderPlaced = AssertPublishedDomainEvent<OrderPlacedEvent>(customer);
            Assert.That(orderPlaced.Value, Is.EqualTo(MoneyValue.Of(200, "EUR")));
        }

        [Test]
        public void PlaceOrder_WhenNoProductIsAdded_BreaksOrderMustHaveAtLeastOneProductRule()
        {
            // Arrange
            Customer customer = CustomerFactory.Create();

            List<OrderProductData> orderProductsData = new List<OrderProductData>();

            List<ProductPriceData> allProductPrices = new List<ProductPriceData>
            {
                SampleProductPrices.Product1EUR, SampleProductPrices.Product1USD
            };

            const string currency = "EUR";
            List<ConversionRate> conversionRates = GetConversionRates();

            // Assert
            AssertBrokenRule<OrderMustHaveAtLeastOneProductRule>(() =>
            {
                // Act
                customer.PlaceOrder(
                    orderProductsData,
                    allProductPrices,
                    currency,
                    conversionRates);
            });
        }

        [Test]
        public void
            PlaceOrder_GivenTwoOrdersInThatDayAlreadyMade_BreaksCustomerCannotOrderMoreThan2OrdersOnTheSameDayRule()
        {
            // Arrange
            Customer customer = CustomerFactory.Create();

            List<OrderProductData> orderProductsData = new List<OrderProductData>();
            orderProductsData.Add(new OrderProductData(SampleProducts.Product1Id, 2));

            List<ProductPriceData> allProductPrices = new List<ProductPriceData>
            {
                SampleProductPrices.Product1EUR, SampleProductPrices.Product1USD
            };

            const string currency = "EUR";
            List<ConversionRate> conversionRates = GetConversionRates();

            SystemClock.Set(new DateTime(2020, 1, 10, 11, 0, 0));
            customer.PlaceOrder(
                orderProductsData,
                allProductPrices,
                currency,
                conversionRates);

            SystemClock.Set(new DateTime(2020, 1, 10, 11, 30, 0));
            customer.PlaceOrder(
                orderProductsData,
                allProductPrices,
                currency,
                conversionRates);

            SystemClock.Set(new DateTime(2020, 1, 10, 12, 00, 0));

            // Assert
            AssertBrokenRule<CustomerCannotOrderMoreThan2OrdersOnTheSameDayRule>(() =>
            {
                // Act
                customer.PlaceOrder(
                    orderProductsData,
                    allProductPrices,
                    currency,
                    conversionRates);
            });
        }

        private static List<ConversionRate> GetConversionRates()
        {
            List<ConversionRate> conversionRates = new List<ConversionRate>();

            conversionRates.Add(new ConversionRate("USD", "EUR", (decimal)0.88));
            conversionRates.Add(new ConversionRate("EUR", "USD", (decimal)1.13));

            return conversionRates;
        }
    }


    public class SampleProducts
    {
        public static readonly ProductId Product1Id = new ProductId(Guid.NewGuid());

        public static readonly ProductId Product2Id = new ProductId(Guid.NewGuid());
    }

    public class SampleProductPrices
    {
        public static readonly ProductPriceData Product1EUR = new ProductPriceData(
            SampleProducts.Product1Id,
            MoneyValue.Of(100, "EUR"));

        public static readonly ProductPriceData Product1USD = new ProductPriceData(
            SampleProducts.Product1Id,
            MoneyValue.Of(110, "USD"));
    }
}