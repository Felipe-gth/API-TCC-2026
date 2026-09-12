namespace Api.Payment.DTOs.Return;

using Api.Payment.Models;

public class PaymentChargeCreatedDto
{
    public int PaymentChargeId { get; set; }
    public string QrCode { get; set; } = null!;
    public string QrCodeBase64 { get; set; } = null!;
    public decimal Amount { get; set; }
    public PaymentChargeStatus Status { get; set; }
}