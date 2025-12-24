using MediatR;

public class ReduceStockCommand : IRequest<bool>
{
    public int ProductId { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }
}