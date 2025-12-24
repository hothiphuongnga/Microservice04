using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Commands.CreateOrder;
using OrderService.Services.Base;

namespace OrderService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderCQRSController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderCQRSController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderCommand command)
    {
        var orderId = await _mediator.Send(command);
        return ResponseEntity.Ok(orderId,"Order created successfully");
    }
}
