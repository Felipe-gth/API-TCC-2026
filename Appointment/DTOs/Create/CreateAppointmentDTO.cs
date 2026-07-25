namespace Api.Appointment.DTOs.Create;
using System.ComponentModel.DataAnnotations;


public class CreateAppointmentDTO
{
    [Required(ErrorMessage = "Type is required")]
    [MaxLength(1, ErrorMessage = "Type Max Lenght 1 characters")]
    public string Type {get; set;}
    [Required(ErrorMessage = "Date and Time is required")]
    public DateTime DateAndTime {get; set;}
    public string? Notes {get; set;}
    [MaxLength(1, ErrorMessage = "Had Treatment Max Lenght 1 characters")]
    public string? HadTreatment {get; set;}
    [MaxLength(40, ErrorMessage = "PhysicalHealth Max Lenght 40 characters")]
    public string? PhysicalHealth {get; set;}
    [MaxLength(1, ErrorMessage = "Marital Status Max Lenght 1 characters")]
    public string? MaritalStatus {get; set;}
    public string? Habits {get; set;}
    public string? SearchReason {get; set;}

    [Required(ErrorMessage = "Patient Id is required")]
    public int PatientId {get; set;}
    [Required(ErrorMessage = "Psychologist Id is required")]
    public int PsychologistId { get; set;}
}