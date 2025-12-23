using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models;

namespace ProductService.Repositories;
public interface IProductCQRSRepository
{
    Task AddAsync(Product product);
    Task<Product?> GetByIdAsync(int id);
    Task SaveChangesAsync();
}
public class ProductCQRSRepository : IProductCQRSRepository
{
     private readonly ProductDbServiceContext _context;

    public ProductCQRSRepository(ProductDbServiceContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }   
}