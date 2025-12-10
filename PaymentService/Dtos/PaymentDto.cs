using System;
using System.Collections.Generic;

namespace PaymentService.Dtos;

public partial class PaymentDto
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int UserId { get; set; }

    public decimal Amount { get; set; }

    public int PaymentMethodId { get; set; }

    public int StatusId { get; set; }

    public string TransactionCode { get; set; } = null!;

    public DateTime PaidAt { get; set; }

    public virtual PaymentMethodDto PaymentMethod { get; set; } = null!;

    public virtual PaymentStatusDto Status { get; set; } = null!;
}
