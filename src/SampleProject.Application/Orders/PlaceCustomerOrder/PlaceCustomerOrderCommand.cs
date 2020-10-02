namespace SampleProject.Application.Orders.PlaceCustomerOrder
{
    using System;
    using System.Collections.Generic;
    using Configuration.Commands;

    public class PlaceCustomerOrderCommand : CommandBase<Guid>
    {
        public PlaceCustomerOrderCommand(
            Guid customerId,
            List<ProductDto> products,
            string currency)
        {
            CustomerId = customerId;
            Products = products;
            Currency = currency;
        }

        public Guid CustomerId { get; }

        public List<ProductDto> Products { get; }

        public string Currency { get; }
    }
}