using System.Text.Json;
using Confluent.Kafka;
using MediatR;
using OrderService.Kafka;
using OrderService.Models;
using OrderService.Repositories;
using OrderService.Repositories.Base;

namespace OrderService.Application.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
{
    private readonly IOrderRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IKafkaProducer _producer;

    public CreateOrderCommandHandler(
        IOrderRepository repo,
        IUnitOfWork uow,
        IKafkaProducer producer)
    {
        _repo = repo;
        _uow = uow;
        _producer = producer;
    }

    public async Task<int> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var order = new Order
        {
            UserId = request.BuyerId,
            Total = request.TotalAmount,
            CreatedAt = DateTime.UtcNow
        };

        foreach (var i in request.OrderDetails)
        {
            order.OrderItems.Add(new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            });
        }

        await _repo.AddAsync(order);
        await _uow.SaveChangesAsync();

        // 🔥 Publish Integration Event
        await _producer.ProduceAsync(
            "order_created_v2",
            new Message<string, string>
            {
                Key = order.Id.ToString(),
                Value = JsonSerializer.Serialize(request.OrderDetails)
            });

        return order.Id;
    }
}
