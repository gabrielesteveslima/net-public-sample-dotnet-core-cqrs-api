namespace SampleProject.API.Orders
{
    using System.Collections.Generic;
    using Application.Orders;

    public class CustomerOrderRequest
    {
        public List<ProductDto> Products { get; set; }

        public string Currency { get; set; }
    }
}