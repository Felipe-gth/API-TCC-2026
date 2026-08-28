namespace Api.Admin.DTOs.Register;
using System.ComponentModel.DataAnnotations;

public class EntryLinkPatientPsychologistDTO
{
    [Range(1, int.MaxValue, ErrorMessage = "PatientId must be a positive number")]
    public int PatientId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "PsychologistId must be a positive number")]
    public int PsychologistId { get; set; }
}