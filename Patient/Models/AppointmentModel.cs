namespace Domain.Models.Appointment;

public class AppointmentModel
{
    public int Id { get; private set; }
    public string Type { get; private set; } 
    public DateTime Date { get; private set; }
    public string Notes { get; private set; }
    public int PatientId { get; private set; }
    public int PsychologistId { get; private set; }

    public AppointmentModel(string type, DateTime date, int patientId, int psychologistId)
    {

        Type = type;
        Date = date;
        PatientId = patientId;
        PsychologistId = psychologistId;
        
    }
}