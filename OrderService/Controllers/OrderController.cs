using Microsoft.AspNetCore.Mvc;
using OrderService.Dtos;
using OrderService.Services;
using OrderService.Services.Base;

namespace OrderService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    
    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // GET: api/orders
    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        var result = await _orderService.GetAllAsync();
        return result;
    }

    // GET: api/orders/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var result = await _orderService.GetByIdAsync(id);
        return result;
    }


    // POST: api/orders
    [HttpPost]
    
    public async Task<IActionResult> CreateOrder(CreateOrderDto orderDto)
    {
        if (!ModelState.IsValid)
        {
            return ResponseEntity.Fail("Invalid model state", 400);
        }

        var result = await _orderService.CreateOrderAsync(orderDto);
        return result;
    }

    // PUT: api/orders/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, OrderDto orderDto)
    {
        if (id != orderDto.Id)
        {
            return ResponseEntity.Fail("ID mismatch", 400);
        }

        var result = await _orderService.UpdateAsync(orderDto);
        return result;
    }

    // DELETE: api/orders/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var result = await _orderService.DeleteAsync(id);
        return result;
    }
}
