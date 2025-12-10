using AutoMapper;
using ProductService.Dtos;
using ProductService.Models;
using ProductService.Repositories;
using ProductService.Repositories.Base;
using ProductService.Services.Base;
namespace ProductService.Services{


public interface IProductService : IServiceBase<Product, ProductDto>
{

}

public class ProductServicee : ServiceBase<Product, ProductDto>, IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductServicee(IUnitOfWork uow, IMapper mapper, IProductRepository productRepository) 
        : base(uow, mapper, productRepository)
    {
        _productRepository = productRepository;
    }

   
}
}