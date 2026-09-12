namespace Api.Payment.Models;

public class PaymentModel
{
    public int id {get; set;}
    public int PatientId {get; set;}
    public int PsychologistId {get; set;}
    public enum PaymentType
    {
        Card = 1, 
        Pix = 2
    }
    public decimal Amount {get; set;}
    public int ReferenceMonth {get; set;}
    public int ReferenceYear {get; set;}
    public string ExternalReference {get; set;}

    public string? MpOrderId { get; set; }
    public string? QrCode { get; set; }
    public string? QrCodeBase64 { get; set; }

    public PaymentChargeStatus Status { get; set; } = PaymentChargeStatus.Pending;
    public DateTime CreatedAt { get; set; }
    public DateTime? PaidAt { get; set; }
}

public enum PaymentChargeStatus
{
    Pending,
    Paid,
    Failed,
    Cancelled
}