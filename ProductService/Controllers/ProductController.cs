using Microsoft.AspNetCore.Mvc;
using ProductService.Dtos;
using ProductService.Services;
using ProductService.Services.Base;

namespace ProductService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _ProductService;
    
    public ProductController(IProductService ProductService)
    {
        _ProductService = ProductService;
    }

    // GET: api/products
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _ProductService.GetAllAsync();
        return result;
    }

    // GET: api/products/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _ProductService.GetByIdAsync(id);
        return result;
    }


    // POST: api/products
    [HttpPost]
    public async Task<IActionResult> Create(ProductDto productDto)
    {
        if (!ModelState.IsValid)
        {
            return ResponseEntity.Fail("Invalid model state", 400);
        }

        var result = await _ProductService.AddAsync(productDto);
        return result;
    }

    // PUT: api/products/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ProductDto productDto)
    {
        if (id != productDto.Id)
        {
            return ResponseEntity.Fail("ID mismatch", 400);
        }

        var result = await _ProductService.UpdateAsync(productDto);
        return result;
    }

    // DELETE: api/products/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _ProductService.DeleteAsync(id);
        return result;
    }
}
