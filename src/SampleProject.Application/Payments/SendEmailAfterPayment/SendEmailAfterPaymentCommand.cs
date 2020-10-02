namespace SampleProject.Application.Payments.SendEmailAfterPayment
{
    using System;
    using Configuration.Commands;
    using Domain.Payments;
    using MediatR;
    using Newtonsoft.Json;

    public class SendEmailAfterPaymentCommand : InternalCommandBase<Unit>
    {
        [JsonConstructor]
        public SendEmailAfterPaymentCommand(Guid id, PaymentId paymentId) : base(id)
        {
            PaymentId = paymentId;
        }

        public PaymentId PaymentId { get; }
    }
}