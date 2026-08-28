namespace Api.Appointment.DTOs.List;
using System.ComponentModel.DataAnnotations;

public class EntryAvailabilityDTO
{
    [Required(ErrorMessage = "Date is required")]
    public DateOnly Date {get; set;}
    [Required(ErrorMessage = "Id is required")]
    [Range(1, int.MaxValue, ErrorMessage = "PsychologistId must be a positive number")]
    public int PsychologistId {get; set;}
}