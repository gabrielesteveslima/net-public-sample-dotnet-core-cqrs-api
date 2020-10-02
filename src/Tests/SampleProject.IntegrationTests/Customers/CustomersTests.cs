namespace SampleProject.IntegrationTests.Customers
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Application.Customers;
    using Application.Customers.GetCustomerDetails;
    using Application.Customers.IntegrationHandlers;
    using Application.Customers.RegisterCustomer;
    using Domain.Customers;
    using Infrastructure.Processing;
    using Infrastructure.Processing.Outbox;
    using NUnit.Framework;
    using SeedWork;

    [TestFixture]
    public class CustomersTests : TestBase
    {
        [Test]
        public async Task RegisterCustomerTest()
        {
            const string email = "newCustomer@mail.com";
            const string name = "Sample Company";

            CustomerDto customer = await CommandsExecutor.Execute(new RegisterCustomerCommand(email, name));
            CustomerDetailsDto customerDetails =
                await QueriesExecutor.Execute(new GetCustomerDetailsQuery(customer.Id));

            Assert.That(customerDetails, Is.Not.Null);
            Assert.That(customerDetails.Name, Is.EqualTo(name));
            Assert.That(customerDetails.Email, Is.EqualTo(email));

            SqlConnection connection = new SqlConnection(ConnectionString);
            List<OutboxMessageDto> messagesList = await OutboxMessagesHelper.GetOutboxMessages(connection);

            Assert.That(messagesList.Count, Is.EqualTo(1));

            CustomerRegisteredNotification customerRegisteredNotification =
                OutboxMessagesHelper.Deserialize<CustomerRegisteredNotification>(messagesList[0]);

            Assert.That(customerRegisteredNotification.CustomerId, Is.EqualTo(new CustomerId(customer.Id)));
        }
    }
}