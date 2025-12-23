using ProductService.Dtos.Commands;
using ProductService.Models;
using ProductService.Repositories;
namespace ProductService.Services;

public interface IProductCommandService
{
    Task<int> CreateAsync(CreateProductCommand cmd);
}
public class ProductCommandService : IProductCommandService
{
    private readonly IProductCQRSRepository _repo;

    public ProductCommandService(IProductCQRSRepository repo)
    {
        _repo = repo;
    }

    public async Task<int> CreateAsync(CreateProductCommand cmd)
    {
        // map DTO to Entity
        var product = new Product
        {
            Name = cmd.Name,
            Price = cmd.Price,
            Stock = cmd.Stock,
            CreatedAt = DateTime.UtcNow
        };

        await _repo.AddAsync(product);
        await _repo.SaveChangesAsync();

        return product.Id; // command trả dữ liệu đơn giản hơn query
    }

  
}
//📌 Command không trả DTO