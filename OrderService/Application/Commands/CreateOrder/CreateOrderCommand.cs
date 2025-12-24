using MediatR;
using OrderService.Dtos;

namespace OrderService.Application.Commands.CreateOrder;

public class CreateOrderCommand : IRequest<int>
{
    public int BuyerId { get; set; }
    public decimal TotalAmount { get; set; }
    public List<ProductOrderDto> OrderDetails { get; set; } = [];
}
