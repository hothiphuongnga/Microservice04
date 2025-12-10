using AutoMapper;
using PaymentService.Dtos;
using PaymentService.Models;
using PaymentService.Repositories;
using PaymentService.Repositories.Base;
using PaymentService.Services.Base;
namespace PaymentService.Services{


public interface IPaymentService : IServiceBase<Payment, PaymentDto>
{

}

public class PaymentServicee : ServiceBase<Payment, PaymentDto>, IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;

    public PaymentServicee(IUnitOfWork uow, IMapper mapper, IPaymentRepository paymentRepository) 
        : base(uow, mapper, paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

   
}
}