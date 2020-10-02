namespace SampleProject.Infrastructure.Emails
{
    using System.Threading.Tasks;
    using Application.Configuration.Emails;

    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(EmailMessage message)
        {
            // Integration with email service.
        }
    }
}