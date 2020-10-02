namespace SampleProject.Application.Orders.ChangeCustomerOrder
{
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
    using MediatR;
    using PlaceCustomerOrder;

    internal sealed class ChangeCustomerOrderCommandHandler : ICommandHandler<ChangeCustomerOrderCommand, Unit>
    {
        private readonly ICustomerRepository _customerRepository;

        private readonly IForeignExchange _foreignExchange;

        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        internal ChangeCustomerOrderCommandHandler(
            ICustomerRepository customerRepository,
            IForeignExchange foreignExchange,
            ISqlConnectionFactory sqlConnectionFactory)
        {
            _customerRepository = customerRepository;
            _foreignExchange = foreignExchange;
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(ChangeCustomerOrderCommand request, CancellationToken cancellationToken)
        {
            Customer customer = await _customerRepository.GetByIdAsync(new CustomerId(request.CustomerId));

            OrderId orderId = new OrderId(request.OrderId);

            List<ConversionRate> conversionRates = _foreignExchange.GetConversionRates();
            List<OrderProductData> orderProducts = request
                .Products
                .Select(x => new OrderProductData(new ProductId(x.Id), x.Quantity))
                .ToList();

            List<ProductPriceData> allProductPrices =
                await ProductPriceProvider.GetAllProductPrices(_sqlConnectionFactory.GetOpenConnection());

            customer.ChangeOrder(
                orderId,
                allProductPrices,
                orderProducts,
                conversionRates,
                request.Currency);

            return Unit.Value;
        }
    }
}