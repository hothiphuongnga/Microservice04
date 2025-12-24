using MediatR;
using ProductService.Repositories;
using ProductService.Repositories.Base;

namespace ProductService.Application.Commands.ReduceStock;

public class ReduceStockCommandHandler
    : IRequestHandler<ReduceStockCommand, bool>
{
    private readonly IProductRepository _repo;
    private readonly IUnitOfWork _uow;

    public ReduceStockCommandHandler(
        IProductRepository repo,
        IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<bool> Handle(
        ReduceStockCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var product = await _repo.GetByIdAsync(request.ProductId);
            if (product == null)
                throw new Exception("Product not found");

            product.Stock += request.Quantity;

            _repo.Update(product);

            await _uow.SaveChangesAsync();

            return true; // 🔥 BẮT BUỘC
        }
        catch (Exception ex)
        {
            // Log lỗi nếu cần
            return false; // 🔥 BẮT BUỘC
        }
    }
}
