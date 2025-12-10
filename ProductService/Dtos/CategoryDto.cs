using System;
using System.Collections.Generic;

namespace ProductService.Dtos;

public partial class CategoryDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
}
