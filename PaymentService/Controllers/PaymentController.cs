using Microsoft.AspNetCore.Mvc;
using PaymentService.Dtos;
using PaymentService.Services;
using PaymentService.Services.Base;

namespace PaymentService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _PaymentService;
    
    public PaymentController(IPaymentService PaymentService)
    {
        _PaymentService = PaymentService;
    }

    // GET: api/payments
    [HttpGet]
    public async Task<IActionResult> GetAllPayments()
    {
        var result = await _PaymentService.GetAllAsync();
        return result;
    }

    // GET: api/payments/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPayment(int id)
    {
        var result = await _PaymentService.GetByIdAsync(id);
        return result;
    }


    // POST: api/payments
    [HttpPost]
    public async Task<IActionResult> CreatePayment(PaymentDto paymentDto)
    {
        if (!ModelState.IsValid)
        {
            return ResponseEntity.Fail("Invalid model state", 400);
        }

        var result = await _PaymentService.AddAsync(paymentDto);
        return result;
    }

    // PUT: api/payments/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePayment(int id, PaymentDto paymentDto)
    {
        if (id != paymentDto.Id)
        {
            return ResponseEntity.Fail("ID mismatch", 400);
        }

        var result = await _PaymentService.UpdateAsync(paymentDto);
        return result;
    }

    // DELETE: api/payments/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePayment(int id)
    {
        var result = await _PaymentService.DeleteAsync(id);
        return result;
    }
}
