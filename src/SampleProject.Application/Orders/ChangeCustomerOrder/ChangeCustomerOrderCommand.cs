namespace SampleProject.Application.Orders.ChangeCustomerOrder
{
    using System;
    using System.Collections.Generic;
    using Configuration.Commands;
    using MediatR;

    public class ChangeCustomerOrderCommand : CommandBase<Unit>
    {
        public ChangeCustomerOrderCommand(
            Guid customerId,
            Guid orderId,
            List<ProductDto> products,
            string currency)
        {
            CustomerId = customerId;
            OrderId = orderId;
            Currency = currency;
            Products = products;
        }

        public Guid CustomerId { get; }

        public Guid OrderId { get; }

        public string Currency { get; }

        public List<ProductDto> Products { get; }
    }
}