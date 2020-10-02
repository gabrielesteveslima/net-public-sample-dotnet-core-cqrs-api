namespace SampleProject.IntegrationTests.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Application.Customers;
    using Application.Customers.IntegrationHandlers;
    using Application.Customers.RegisterCustomer;
    using Application.Orders;
    using Application.Orders.GetCustomerOrderDetails;
    using Application.Orders.PlaceCustomerOrder;
    using Application.Payments;
    using Domain.Customers;
    using Domain.Customers.Orders;
    using Infrastructure.Processing;
    using Infrastructure.Processing.Outbox;
    using NUnit.Framework;
    using SeedWork;

    [TestFixture]
    public class OrdersTests : TestBase
    {
        [Test]
        public async Task PlaceOrder_Test()
        {
            string customerEmail = "email@email.com";
            CustomerDto customer =
                await CommandsExecutor.Execute(new RegisterCustomerCommand(customerEmail, "Sample Customer"));

            List<ProductDto> products = new List<ProductDto>();
            Guid productId = Guid.Parse("9DB6E474-AE74-4CF5-A0DC-BA23A42E2566");
            products.Add(new ProductDto(productId, 2));
            Guid orderId = await CommandsExecutor.Execute(new PlaceCustomerOrderCommand(customer.Id, products, "EUR"));

            OrderDetailsDto orderDetails = await QueriesExecutor.Execute(new GetCustomerOrderDetailsQuery(orderId));

            Assert.That(orderDetails, Is.Not.Null);
            Assert.That(orderDetails.Value, Is.EqualTo(70));
            Assert.That(orderDetails.Products.Count, Is.EqualTo(1));
            Assert.That(orderDetails.Products[0].Quantity, Is.EqualTo(2));
            Assert.That(orderDetails.Products[0].Id, Is.EqualTo(productId));

            SqlConnection connection = new SqlConnection(ConnectionString);
            List<OutboxMessageDto> messagesList = await OutboxMessagesHelper.GetOutboxMessages(connection);

            Assert.That(messagesList.Count, Is.EqualTo(3));

            CustomerRegisteredNotification customerRegisteredNotification =
                OutboxMessagesHelper.Deserialize<CustomerRegisteredNotification>(messagesList[0]);

            Assert.That(customerRegisteredNotification.CustomerId, Is.EqualTo(new CustomerId(customer.Id)));

            OrderPlacedNotification orderPlaced =
                OutboxMessagesHelper.Deserialize<OrderPlacedNotification>(messagesList[1]);

            Assert.That(orderPlaced.OrderId, Is.EqualTo(new OrderId(orderId)));

            PaymentCreatedNotification paymentCreated =
                OutboxMessagesHelper.Deserialize<PaymentCreatedNotification>(messagesList[2]);

            Assert.That(paymentCreated, Is.Not.Null);
        }
    }
}