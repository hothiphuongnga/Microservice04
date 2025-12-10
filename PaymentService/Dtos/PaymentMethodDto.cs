using System;
using System.Collections.Generic;

namespace PaymentService.Dtos;

public partial class PaymentMethodDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

}
