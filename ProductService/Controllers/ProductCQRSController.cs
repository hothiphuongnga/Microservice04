namespace ProductService.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using ProductService.Application.Commands.CreateProduct;
    using ProductService.Application.Queries.GetProductById;
    using ProductService.Services.Base;

    // Decoupled :  giarm suwj chặc chẽ giauwx các file

    // repository partern -> service layer -> controller layer 
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCQRSController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductCQRSController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // COMMAND: Tạo mới sản phẩm
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductCommand command) // nhận CreateProductCommand từ body request
        {
            var productId = await _mediator.Send(command);
            return ResponseEntity.Ok(productId,"Product created successfully");
        }

        // QUERY: Lấy thông tin sản phẩm theo Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetProductByIdQuery(id);
           var product = await _mediator.Send(query);
           return ResponseEntity.Ok(product!,"Product retrieved successfully");
        }

    }
}