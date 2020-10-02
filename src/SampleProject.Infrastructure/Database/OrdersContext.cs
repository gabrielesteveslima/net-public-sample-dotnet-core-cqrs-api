namespace SampleProject.Infrastructure.Database
{
    using Microsoft.EntityFrameworkCore;
    using Processing.InternalCommands;
    using Processing.Outbox;
    using SampleProject.Domain.Customers;
    using SampleProject.Domain.Payments;
    using SampleProject.Domain.Products;

    public class OrdersContext : DbContext
    {
        public OrdersContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        public DbSet<InternalCommand> InternalCommands { get; set; }

        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrdersContext).Assembly);
        }
    }
}