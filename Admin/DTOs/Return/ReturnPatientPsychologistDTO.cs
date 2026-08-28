namespace Api.Admin.DTOs.Return;
public class ReturnPatientPsychologistDTO
{
    public int PatientId { get; set; }
    public int PsychologistId { get; set; }
    public string PsychologistName { get; set; } = string.Empty;
    public bool Active { get; set; }
}