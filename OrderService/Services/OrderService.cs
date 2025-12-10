using AutoMapper;
using OrderService.Dtos;
using OrderService.Models;
using OrderService.Repositories;
using OrderService.Repositories.Base;
using OrderService.Services.Base;
namespace OrderService.Services{


public interface IOrderService : IServiceBase<Order, OrderDto>
{

}

public class OrderServicee : ServiceBase<Order, OrderDto>, IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderServicee(IUnitOfWork uow, IMapper mapper, IOrderRepository orderRepository) 
        : base(uow, mapper, orderRepository)
    {
        _orderRepository = orderRepository;
    }

   
}
}