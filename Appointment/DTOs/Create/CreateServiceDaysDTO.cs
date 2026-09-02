namespace Api.Appointment.DTOs.Create;
using System.ComponentModel.DataAnnotations;
using Api.Psychologist.DTOs.Register;

public class CreateServiceDaysDTO
{
    [Required(ErrorMessage = "PsychologistId is required")]
    [Range(1, int.MaxValue, ErrorMessage = "PsychologistId must be a positive number")]
    public int PsychologistId { get; set; }

    [Required(ErrorMessage = "Days are required")]
    [MinLength(1, ErrorMessage = "At least one day must be informed")]
    public List<CreateServiceDayDTO> Days { get; set; } = new();
}
