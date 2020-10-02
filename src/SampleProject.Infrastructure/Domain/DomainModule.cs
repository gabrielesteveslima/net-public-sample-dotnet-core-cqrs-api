namespace SampleProject.Infrastructure.Domain
{
    using Application.Customers.DomainServices;
    using Autofac;
    using ForeignExchanges;
    using SampleProject.Domain.Customers;
    using SampleProject.Domain.ForeignExchange;

    public class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CustomerUniquenessChecker>()
                .As<ICustomerUniquenessChecker>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ForeignExchange>()
                .As<IForeignExchange>()
                .InstancePerLifetimeScope();
        }
    }
}