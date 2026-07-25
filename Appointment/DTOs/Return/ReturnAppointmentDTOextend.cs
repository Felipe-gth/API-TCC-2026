namespace Api.Appointment.DTOs.Return;


public class ReturnAppointmentDTOextend
{
    public int Id {get; set; }
    public string Type {get; set;}
    public int Day {get; set;}
    public int Month {get; set;}
    public int Hour {get; set;}
    public string Notes {get; set;}
    public string HadTreatment {get; set;}
    public string PhysicalHealth {get; set;}
    public string MaritalStatus {get; set;}
    public string Habits {get; set;}
    public string SearchReason {get; set;}
    public int PatientId {get; set;}
    public int PsychologistId { get; set;}
}

