using PaymentService.Data;
using PaymentService.Models;
using PaymentService.Repositories.Base;

namespace PaymentService.Repositories;
public interface IPaymentRepository : IRepositoryBase<Payment>
{
    
}
public class PaymentRepository : RepositoryBase<Payment>, IPaymentRepository
{
    public PaymentRepository(PaymentDbServiceContext context) : base(context)
    {
    }
}