using AutoMapper;
using OrderService.Dtos;
using OrderService.Models;

public class OrderMapping : Profile
{
    public OrderMapping()
    {
        CreateMap<Order,OrderDto>()
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
        
        CreateMap<CreateOrderDto, Order>().ReverseMap();


        // order item
        CreateMap<OrderItem,OrderItemDto>();
        
        CreateMap<OrderDto, Order>()
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
        
        CreateMap<OrderItemDto, OrderItem>();
    }
}