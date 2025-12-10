using System;
using System.Collections.Generic;

namespace ProductService.Dtos;

public partial class ProductImageDto
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string Url { get; set; } = null!;

}
