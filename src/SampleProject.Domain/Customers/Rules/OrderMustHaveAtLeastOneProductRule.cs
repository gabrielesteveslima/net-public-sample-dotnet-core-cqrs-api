﻿namespace SampleProject.Domain.Customers.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using Orders;
    using SeedWork;

    public class OrderMustHaveAtLeastOneProductRule : IBusinessRule
    {
        private readonly List<OrderProductData> _orderProductData;

        public OrderMustHaveAtLeastOneProductRule(List<OrderProductData> orderProductData)
        {
            _orderProductData = orderProductData;
        }

        public bool IsBroken()
        {
            return !_orderProductData.Any();
        }

        public string Message => "Order must have at least one product";
    }
}