namespace SampleProject.Domain.Payments
{
    using System;
    using SeedWork;

    public class PaymentId : TypedIdValueBase
    {
        public PaymentId(Guid value) : base(value)
        {
        }
    }
}