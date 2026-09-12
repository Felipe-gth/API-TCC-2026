namespace Api.Payment.DTOs.Entry;

public class EntryDataPayment
{
    public enum PaymentType
    {
        Card = 1, 
        Pix = 2
    }
    public int PatientId { get; set; }
    public int PsychologistId {get;set;}
    public DateOnly Date {get; set;}
    
}