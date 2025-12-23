namespace ProductService.Controllers
{

    using Microsoft.AspNetCore.Mvc;
    using ProductService.Dtos.Commands;
    using ProductService.Services;
    using ProductService.Services.Base;

    [Route("api/[controller]")]
    [ApiController]
    public class ProductCommandController : ControllerBase
    {

        private readonly IProductCommandService _service;
        public ProductCommandController(IProductCommandService service)
        {
            _service = service;
        }
        // Các action liên quan đến command (tạo, cập nhật, xóa sản phẩm) sẽ được định nghĩa ở đây
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateProductCommand cmd)
        {
            var res = await _service.CreateAsync(cmd);
            return  ResponseEntity.Ok(res, "Product created successfully");
            
        }
    }
}
// command không trả DTO , chỉ trả dữ liệu đơn giản hơn query