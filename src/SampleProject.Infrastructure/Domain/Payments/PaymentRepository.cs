namespace SampleProject.Infrastructure.Domain.Payments
{
    using System;
    using System.Threading.Tasks;
    using Database;
    using Microsoft.EntityFrameworkCore;
    using SampleProject.Domain.Payments;

    public class PaymentRepository : IPaymentRepository
    {
        private readonly OrdersContext _context;

        public PaymentRepository(OrdersContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Payment> GetByIdAsync(PaymentId id)
        {
            return await _context.Payments
                .SingleAsync(x => x.Id == id);
        }

        public async Task AddAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
        }
    }
}