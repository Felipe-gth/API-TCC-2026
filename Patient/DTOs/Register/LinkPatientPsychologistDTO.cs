namespace Api.Patient.DTOs.Register;
using System.ComponentModel.DataAnnotations;

public class LinkPatientPsychologistDTO
{
    [Required(ErrorMessage = "PatientId is required")]
    [Range(1, int.MaxValue, ErrorMessage = "PatientId must be a positive number")]
    public int PatientId { get; set; }

    [Required(ErrorMessage = "PsychologistId is required")]
    [Range(1, int.MaxValue, ErrorMessage = "PsychologistId must be a positive number")]
    public int PsychologistId { get; set; }
}
