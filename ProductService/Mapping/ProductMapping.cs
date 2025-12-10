using AutoMapper;
using ProductService.Dtos;
using ProductService.Models;
namespace ProductService.Mapping;
public class ProductMapping : Profile
{
    public ProductMapping()
    {

        CreateMap<Product,ProductDto>().ReverseMap();
        // category
        CreateMap<Category,CategoryDto>().ReverseMap();
        
        // product image
        CreateMap<ProductImage,ProductImageDto>().ReverseMap();;
        
    }
}