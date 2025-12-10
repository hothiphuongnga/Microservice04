using System;
using System.Collections.Generic;

namespace ProductService.Dtos;

public partial class ProductDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public int? CategoryId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<ProductImageDto> ProductImages { get; set; } = new List<ProductImageDto>();
}
