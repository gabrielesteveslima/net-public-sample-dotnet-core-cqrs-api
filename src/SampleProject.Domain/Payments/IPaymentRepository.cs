namespace SampleProject.Domain.Payments
{
    using System.Threading.Tasks;

    public interface IPaymentRepository
    {
        Task<Payment> GetByIdAsync(PaymentId id);

        Task AddAsync(Payment payment);
    }
}