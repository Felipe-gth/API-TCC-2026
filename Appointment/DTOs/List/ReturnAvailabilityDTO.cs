namespace Api.Appointment.DTOs.List;
using Api.Appointment.DTOs;

public class ReturnAvailabilityDTO
{
    public int PsychologistId {get; set;}
    public DateOnly Date {get; set;}
    public int WeekDay {get; set;}
    public bool Avaliability {get; set;}
    public List<HourAvailabilityDTO> Hours { get; set; } = new();
}