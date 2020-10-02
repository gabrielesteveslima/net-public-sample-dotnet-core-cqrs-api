namespace SampleProject.Infrastructure.Database
{
    using Application.Configuration.Data;
    using Autofac;
    using Domain;
    using Domain.Customers;
    using Domain.Payments;
    using Domain.Products;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using SampleProject.Domain.Customers.Orders;
    using SampleProject.Domain.Payments;
    using SampleProject.Domain.Products;
    using SampleProject.Domain.SeedWork;
    using SeedWork;

    public class DataAccessModule : Module
    {
        private readonly string _databaseConnectionString;

        public DataAccessModule(string databaseConnectionString)
        {
            _databaseConnectionString = databaseConnectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SqlConnectionFactory>()
                .As<ISqlConnectionFactory>()
                .WithParameter("connectionString", _databaseConnectionString)
                .InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();


            builder.RegisterType<CustomerRepository>()
                .As<ICustomerRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ProductRepository>()
                .As<IProductRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PaymentRepository>()
                .As<IPaymentRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<StronglyTypedIdValueConverterSelector>()
                .As<IValueConverterSelector>()
                .InstancePerLifetimeScope();

            builder
                .Register(c =>
                {
                    DbContextOptionsBuilder<OrdersContext> dbContextOptionsBuilder =
                        new DbContextOptionsBuilder<OrdersContext>();
                    dbContextOptionsBuilder.UseSqlServer(_databaseConnectionString);
                    dbContextOptionsBuilder
                        .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();

                    return new OrdersContext(dbContextOptionsBuilder.Options);
                })
                .AsSelf()
                .As<DbContext>()
                .InstancePerLifetimeScope();
        }
    }
}