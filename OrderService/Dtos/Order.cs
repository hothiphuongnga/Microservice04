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

public class CreateOrderDto
{
    public int BuyerId { get; set; }

    public decimal TotalAmount { get; set; }

    public List<ProductOrderDto> OrderDetails { get; set; } = new List<ProductOrderDto>();
}

public class ProductOrderDto
{
    public int ProductId { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }
}