using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Dtos.Commands;
using ProductService.Dtos.Queries;

namespace ProductService.Services;

public interface IProductQueryService
{
    Task<ProductQueryDetail?> GetByIdAsync(int id);
}

public class ProductQueryService : IProductQueryService
{
    private readonly ProductDbServiceContext _context;

    public ProductQueryService(ProductDbServiceContext context)
    {
        _context = context;
    }
    public async Task<ProductQueryDetail?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Where(x => x.Id == id)
            .Select(x => new ProductQueryDetail
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Stock = x.Stock
            })
            .FirstOrDefaultAsync();
    }
}
//📌 Không cần Repository
//📌 Không cần Entity return