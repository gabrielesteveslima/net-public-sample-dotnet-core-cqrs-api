namespace SampleProject.Application.Customers.GetCustomerDetails
{
    using System;
    using Configuration.Queries;

    public class GetCustomerDetailsQuery : IQuery<CustomerDetailsDto>
    {
        public GetCustomerDetailsQuery(Guid customerId)
        {
            CustomerId = customerId;
        }

        public Guid CustomerId { get; }
    }
}