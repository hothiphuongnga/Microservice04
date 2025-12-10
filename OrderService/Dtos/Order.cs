using System;
using System.Collections.Generic;

namespace OrderService.Dtos;

public class OrderDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public decimal Total { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
}
