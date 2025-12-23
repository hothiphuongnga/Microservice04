// MỤC TIÊU 
/*
Tách api đọc / ghi  (query / command) 
Chung DB , Chung project tách service

ví dụ basic không có kafka, eventbus, không mediatR
*/

using Microsoft.AspNetCore.Mvc;
using ProductService.Services;

namespace ProductService.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ProductQueryController : ControllerBase
{
    private readonly IProductQueryService _service;

    public ProductQueryController(IProductQueryService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _service.GetByIdAsync(id);
        return product == null ? NotFound() : Ok(product);
    }
}
// query không cần gọi repo vì repo thuộc về domain write 
