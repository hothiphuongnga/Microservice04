namespace ProductService.Dtos.Queries;

public class ProductQueryDetail
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}
//📌 DTO đọc không chứa logic