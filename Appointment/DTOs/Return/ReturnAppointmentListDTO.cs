namespace Api.Appointment.DTOs.Return;

public class ReturnAppointmentListDTO
{
    public int Id { get; set; }
    public string Type { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; }
    public int PatientId { get; set; }
    public int PsychologistId { get; set; }
    public string PatientName { get; set; }
    public string PatientLastName { get; set; }
    public string PsychologistName { get; set; }
    public string PsychologistLastName { get; set; }
}
