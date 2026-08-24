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

    public ReturnAppointmentDTOextend(int id, string type, int day, int month, int hour, string? notes, string? maritalStatus, string? physicalHealth, string? hadTreatment, string? habits, string? searchReason, int patientId, int psychologistId)
    {
        Id = id;
        Type = type;
        Day = day;
        Month = month;
        Hour = hour;
        Notes = notes;
        MaritalStatus = maritalStatus;
        PhysicalHealth = physicalHealth;
        HadTreatment = hadTreatment;
        Habits = habits;
        SearchReason = searchReason;
        PatientId = patientId;
        PsychologistId = psychologistId;
    }
}

