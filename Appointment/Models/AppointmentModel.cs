namespace Api.Appointment.Models;

public class AppointmentModel
{
    public int? ind { get; private set; }
    public string Type { get; private set; } 
    public DateTime Date { get; private set; }
    public string? Notes { get; private set; }
    public string? MaritalStatus {get; private set; }
    public string? PhysicalHealth {get; private set; }
    public string? HadTreatment {get; private set; }
    public string? Habits {get; private set; }
    public string? SearchReason {get; private set; }
    public int PatientId { get; private set; }
    public int PsychologistId { get; private set; }

    public AppointmentModel()
    {
        
    }

    public AppointmentModel(string type, DateTime date, string? notes, string? maritalStatus, string? physicalHealth, string? hadTreatment, string? habits, string? searchReason, int patientId, int psychologistId)
    {
        Type = type;
        Date = date;
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