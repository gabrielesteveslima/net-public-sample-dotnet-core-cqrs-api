namespace SampleProject.Application.Customers.RegisterCustomer
{
    using Configuration.Commands;

    public class RegisterCustomerCommand : CommandBase<CustomerDto>
    {
        public RegisterCustomerCommand(string email, string name)
        {
            Email = email;
            Name = name;
        }

        public string Email { get; }

        public string Name { get; }
    }
}