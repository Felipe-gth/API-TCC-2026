namespace Api.Appointment.DTOs.Return;

public class ReturnAppointmentDTOsimple
{
    public int Id {get; set; }
    public string Type {get; set;}
    public int Day {get; set;}
    public int Month {get; set;}
    public int Hour {get; set;}
    public int PatientId {get; set;}
    public int PsychologistId { get; set;}

    public ReturnAppointmentDTOsimple(int id, string type, int day, int month, int hour, int patientId, int psychologistId)
    {
        Id = id;
        Type = type;
        Day = day;
        Month = month;
        Hour = hour;
        PatientId = patientId;
        PsychologistId = psychologistId;
    }
}