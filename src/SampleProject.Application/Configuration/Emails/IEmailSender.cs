namespace SampleProject.Application.Configuration.Emails
{
    using System.Threading.Tasks;

    public interface IEmailSender
    {
        Task SendEmailAsync(EmailMessage message);
    }
}