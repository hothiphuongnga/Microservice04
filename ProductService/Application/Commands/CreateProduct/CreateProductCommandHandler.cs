using MediatR;
using ProductService.Services;

namespace ProductService.Application.Commands.CreateProduct;

public class CreateProductCommandHandler
    : IRequestHandler<CreateProductCommand, int>
{
    private readonly IProductService _productService;

    public CreateProductCommandHandler(
        IProductService productService)
    {
        _productService = productService;
    }

    public async Task<int> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
    // gọi service để tạo mới sản phẩm
        var productId = await _productService.CreateProductAsync(
            request.Name,
            request.Price,
            request.Stock);


        return productId;
    }
}
