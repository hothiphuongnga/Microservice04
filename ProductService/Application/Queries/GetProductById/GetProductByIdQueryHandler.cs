using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Dtos;

namespace ProductService.Application.Queries.GetProductById;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly ProductDbServiceContext _context;

    public GetProductByIdQueryHandler(ProductDbServiceContext context)
    {
        _context = context;
    }

    public async Task<ProductDto?> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Products
            .AsNoTracking() // tối ưu truy vấn chỉ đọc, 
            .Where(x => x.Id == request.Id)
            .Select(x => new ProductDto
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Stock = x.Stock,
                CategoryId = x.CategoryId
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
