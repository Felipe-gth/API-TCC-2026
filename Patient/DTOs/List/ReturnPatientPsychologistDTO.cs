namespace Api.Patient.DTOs.List;

public class ReturnPatientPsychologistDTO
{
    public int PatientId { get; set; }
    public int PsychologistId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
}
