namespace SampleProject.Infrastructure.Domain.Customers
{
    using System;
    using System.Threading.Tasks;
    using Database;
    using Microsoft.EntityFrameworkCore;
    using SampleProject.Domain.Customers;
    using SampleProject.Domain.Customers.Orders;
    using SeedWork;

    public class CustomerRepository : ICustomerRepository
    {
        private readonly OrdersContext _context;

        public CustomerRepository(OrdersContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
        }

        public async Task<Customer> GetByIdAsync(CustomerId id)
        {
            return await _context.Customers
                .IncludePaths(
                    CustomerEntityTypeConfiguration.OrdersList,
                    CustomerEntityTypeConfiguration.OrderProducts)
                .SingleAsync(x => x.Id == id);
        }
    }
}