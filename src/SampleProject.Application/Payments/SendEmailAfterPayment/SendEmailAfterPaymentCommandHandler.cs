namespace SampleProject.Application.Payments.SendEmailAfterPayment
{
    using System.Threading;
    using System.Threading.Tasks;
    using Configuration.Commands;
    using Configuration.Emails;
    using Domain.Payments;
    using MediatR;

    public class SendEmailAfterPaymentCommandHandler : ICommandHandler<SendEmailAfterPaymentCommand, Unit>
    {
        private readonly IEmailSender _emailSender;
        private readonly IPaymentRepository _paymentRepository;

        public SendEmailAfterPaymentCommandHandler(
            IEmailSender emailSender,
            IPaymentRepository paymentRepository)
        {
            _emailSender = emailSender;
            _paymentRepository = paymentRepository;
        }

        public async Task<Unit> Handle(SendEmailAfterPaymentCommand request, CancellationToken cancellationToken)
        {
            // Logic of preparing an email. This is only mock.
            EmailMessage emailMessage = new EmailMessage("from@email.com", "to@email.com", "content");

            await _emailSender.SendEmailAsync(emailMessage);

            Payment payment = await _paymentRepository.GetByIdAsync(request.PaymentId);

            payment.MarkEmailNotificationIsSent();

            return Unit.Value;
        }
    }
}