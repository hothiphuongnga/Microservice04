namespace ProductService.Dtos.Commands;
public class CreateProductCommand
{
     public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int Stock { get; set; }
}
