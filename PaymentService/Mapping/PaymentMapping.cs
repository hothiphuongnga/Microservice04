using AutoMapper;
using PaymentService.Dtos;
using PaymentService.Models;

public class PaymentMapping : Profile
{
    public PaymentMapping()
    {
        CreateMap<Payment,PaymentDto>()
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        
        CreateMap<PaymentDto, Payment>()
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
    }
}