namespace SampleProject.UnitTests.Customers
{
    using Domain.Customers;
    using Domain.Customers.Rules;
    using NSubstitute;
    using NUnit.Framework;
    using SeedWork;

    [TestFixture]
    public class CustomerRegistrationTests : TestBase
    {
        [Test]
        public void GivenCustomerEmailIsUnique_WhenCustomerIsRegistering_IsSuccessful()
        {
            // Arrange
            ICustomerUniquenessChecker customerUniquenessChecker = Substitute.For<ICustomerUniquenessChecker>();
            const string email = "testEmail@email.com";
            customerUniquenessChecker.IsUnique(email).Returns(true);

            // Act
            Customer customer = Customer.CreateRegistered(email, "Sample name", customerUniquenessChecker);

            // Assert
            AssertPublishedDomainEvent<CustomerRegisteredEvent>(customer);
        }

        [Test]
        public void GivenCustomerEmailIsNotUnique_WhenCustomerIsRegistering_BreaksCustomerEmailMustBeUniqueRule()
        {
            // Arrange
            ICustomerUniquenessChecker customerUniquenessChecker = Substitute.For<ICustomerUniquenessChecker>();
            const string email = "testEmail@email.com";
            customerUniquenessChecker.IsUnique(email).Returns(false);

            // Assert
            AssertBrokenRule<CustomerEmailMustBeUniqueRule>(() =>
            {
                // Act
                Customer.CreateRegistered(email, "Sample name", customerUniquenessChecker);
            });
        }
    }
}