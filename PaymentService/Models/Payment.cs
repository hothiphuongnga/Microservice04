using System;
using System.Collections.Generic;

namespace PaymentService.Models;

public partial class Payment
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int UserId { get; set; }

    public decimal Amount { get; set; }

    public int PaymentMethodId { get; set; }

    public int StatusId { get; set; }

    public string TransactionCode { get; set; } = null!;

    public DateTime PaidAt { get; set; }

    public virtual PaymentMethod PaymentMethod { get; set; } = null!;

    public virtual PaymentStatus Status { get; set; } = null!;
}
