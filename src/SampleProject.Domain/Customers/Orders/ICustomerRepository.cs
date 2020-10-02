namespace SampleProject.Domain.Customers.Orders
{
    using System.Threading.Tasks;

    public interface ICustomerRepository
    {
        Task<Customer> GetByIdAsync(CustomerId id);

        Task AddAsync(Customer customer);
    }
}