namespace Api.Appointment.DTOs.Update;
using System.ComponentModel.DataAnnotations;

public class EntryUpdateAppointmentStatusDTO
{
    [Required(ErrorMessage = "Status is required")]
    [EnumDataType(typeof(AppointmentStatus), ErrorMessage = "Invalid status value")]
    public AppointmentStatus Status { get; set; }
}

public enum AppointmentStatus
{
    Pendente,
    Finalizado,
    Cancelado
}