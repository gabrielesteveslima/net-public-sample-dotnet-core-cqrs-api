namespace SampleProject.Application.Orders.PlaceCustomerOrder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Configuration.Commands;
    using Configuration.Data;
    using Domain.Customers;
    using Domain.Customers.Orders;
    using Domain.ForeignExchange;
    using Domain.Products;

    public class PlaceCustomerOrderCommandHandler : ICommandHandler<PlaceCustomerOrderCommand, Guid>
    {
        private readonly ICustomerRepository _customerRepository;

        private readonly IForeignExchange _foreignExchange;

        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public PlaceCustomerOrderCommandHandler(
            ICustomerRepository customerRepository,
            IForeignExchange foreignExchange,
            ISqlConnectionFactory sqlConnectionFactory)
        {
            _customerRepository = customerRepository;
            _foreignExchange = foreignExchange;
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Guid> Handle(PlaceCustomerOrderCommand command, CancellationToken cancellationToken)
        {
            Customer customer = await _customerRepository.GetByIdAsync(new CustomerId(command.CustomerId));

            List<ProductPriceData> allProductPrices =
                await ProductPriceProvider.GetAllProductPrices(_sqlConnectionFactory.GetOpenConnection());

            List<ConversionRate> conversionRates = _foreignExchange.GetConversionRates();

            List<OrderProductData> orderProductsData = command
                .Products
                .Select(x => new OrderProductData(new ProductId(x.Id), x.Quantity))
                .ToList();

            OrderId orderId = customer.PlaceOrder(
                orderProductsData,
                allProductPrices,
                command.Currency,
                conversionRates);

            return orderId.Value;
        }
    }
}