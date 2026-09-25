namespace Api.Payment.Controllers;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Result;
using Api.Payment.Interfaces;
using Api.Payment.DTOs.Return;
using Api.Payment.DTOs.Entry;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentInterface _IPayment;
    public PaymentController(IPaymentInterface IPayment)
    {
        _IPayment = IPayment;
    }

    [HttpPost("CreatePayment")]
    public async Task<ActionResult<Result<PaymentChargeCreatedDto>>> CreatePaymentCharge([FromBody] EntryDataPayment dto)
    {
        try
        {
            var result = await _IPayment.CreatePaymentCharge(dto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result); // manda o Result inteiro, não só uma string — assim o front pega a Message
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}