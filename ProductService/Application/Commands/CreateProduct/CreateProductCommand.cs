// class , dto , 
namespace ProductService.Application.Commands.CreateProduct;
using MediatR;


public class CreateProductCommand : IRequest<int> 
//CreateProductCommand kế thừa IRequest<int> 
//  int là kiểu dữ liệu trả về (ProductId) khi tạo mới sản phẩm
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}
