using AutoMapper;
using ProductService.Dtos;
using ProductService.Models;
using ProductService.Repositories;
using ProductService.Repositories.Base;
using ProductService.Services.Base;
namespace ProductService.Services
{


    public interface IProductService : IServiceBase<Product, ProductDto>
    {
        Task<ResponseEntity> UpdateStockAsync(int productId, int quantity);
    }

    public class ProductServicee : ServiceBase<Product, ProductDto>, IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductServicee(IUnitOfWork uow, IMapper mapper, IProductRepository productRepository)
            : base(uow, mapper, productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ResponseEntity> UpdateStockAsync(int productId, int quantity)
        {
            // ktsp
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return ResponseEntity.Fail("Product not found", 404);
            }
            // cập nhật tồn kho
            // kiểm tra tồn kho đủ ko
            if (product.Stock < quantity)
            {
                return ResponseEntity.Fail("Out of stock", 400);
            }
            product.Stock += quantity;
            // lưu thay đổi
            _productRepository.Update(product);
            await _uow.SaveChangesAsync();
            return ResponseEntity.Ok(product);

        }
    }
}

// stock += -9 =>   stock = stock + (-9)