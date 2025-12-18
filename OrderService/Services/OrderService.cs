using System.Text.Json;
using AutoMapper;
using Confluent.Kafka;
using OrderService.Dtos;
using OrderService.Kafka;
using OrderService.Models;
using OrderService.Repositories;
using OrderService.Repositories.Base;
using OrderService.Services.Base;
namespace OrderService.Services{


public interface IOrderService : IServiceBase<Order, OrderDto>
{
    Task<ResponseEntity> CreateOrderAsync(CreateOrderDto createOrderDto);

}

public class OrderServicee : ServiceBase<Order, OrderDto>, IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IHttpClientFactory _factory;
    private readonly IKafkaProducer _kafkaProducer;

    public OrderServicee(IUnitOfWork uow, IMapper mapper, IOrderRepository orderRepository, IHttpClientFactory factory,
        IKafkaProducer kafkaProducer) 
        : base(uow, mapper, orderRepository)
    {
        _orderRepository = orderRepository;
        _factory = factory;
        _kafkaProducer = kafkaProducer;
    }

        public async Task<ResponseEntity> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            var order = new Order(){
                Id = 0,
                UserId = createOrderDto.BuyerId,
                Total = createOrderDto.TotalAmount,
                CreatedAt = DateTime.Now
            };
            // chạy vòng lặp để thêm sp vào order items
            foreach (var item in createOrderDto.OrderDetails)
            {
                var odDetail = new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                };
                order.OrderItems.Add(odDetail);

            }
             // lưu order vào db
            await _orderRepository.AddAsync(order);
            await _uow.SaveChangesAsync();
            // gửi message đến Kafka để thông báo đơn hàng mới được tạo
            var mess = new Message<string,string>
            {
                Key = order.Id.ToString(),
                Value = JsonSerializer.Serialize(createOrderDto.OrderDetails)
            };
            await _kafkaProducer.ProduceAsync("order_created", mess);
    
            //  update tồn kho


            // var client = _factory.CreateClient("ProductService");
            // // đặt nhiều sp cùng lúc 
            // foreach (var item in createOrderDto.OrderDetails)
            // {
            //     // goi api update tồng kho
            //     var response = await client.PutAsync($"/api/Product/{item.ProductId}/stock?quantity={-item.Quantity}", null);

            //     // kiểm tra respon nếu dúng thì tiếp tục vongf lặp còn sai thì upadte trạng của donhang
            //     if (!response.IsSuccessStatusCode)
            //     {
            //         return ResponseEntity.Fail($"Cập nhật tồn kho cho sản phẩm {item.ProductId} thất bại", 400);
            //     }
            // }
            return ResponseEntity.Ok(_map.Map<OrderDto>(order));
        }
    }
}