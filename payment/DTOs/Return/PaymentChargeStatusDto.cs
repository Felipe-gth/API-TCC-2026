namespace Api.Payment.DTOs.Return;
using Api.Payment.Models;

public class PaymentChargeStatusDto
{
    public int PaymentChargeId { get; set; }
    public PaymentChargeStatus Status { get; set; }
    public DateTime? PaidAt { get; set; }
    public decimal Amount {get; set;}
}