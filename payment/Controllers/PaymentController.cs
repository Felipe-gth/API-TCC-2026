namespace Api.Payment.Controllers;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Result;
using Api.Payment.Interfaces;
using Api.Payment.DTOs.Return;
using Api.Payment.DTOs.Entry;

[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaymentInterface _IPayment;

    public PaymentController(IPaymentInterface IPayment)
    {
        _IPayment = IPayment;
    }

    [HttpPost("CreatePayment")]
    public async Task<ActionResult<Result<PaymentChargeCreatedDto>>> CreatePaymentCharge(
        [FromBody] EntryDataPayment dto)
    {
        var result = await _IPayment.CreatePaymentCharge(dto);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpGet("/api/payment/test")]
    public IActionResult Test()
    {
        return Ok("API funcionando");
    }

[HttpPost("/api/payment/webhook")]
public async Task<IActionResult> Webhook()
{
    var orderId = Request.Query["data.id"].FirstOrDefault();
    var type = Request.Query["type"].FirstOrDefault();

    Console.WriteLine("=================================");
    Console.WriteLine("WEBHOOK RECEBIDO!");
    Console.WriteLine($"Type: {type}");
    Console.WriteLine($"Order ID: {orderId}");
    Console.WriteLine("=================================");

    if (type != "order")
        return Ok();

    if (string.IsNullOrWhiteSpace(orderId))
        return BadRequest();

    await _IPayment.ProcessWebhook(orderId);

    return Ok();
}
}