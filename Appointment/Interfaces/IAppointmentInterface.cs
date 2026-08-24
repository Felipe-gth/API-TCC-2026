namespace Api.Appointment.Interface;

using Api.Appointment.DTOs.Create;
using Api.Appointment.DTOs.Return;
using Api.Shared.DTOs.Result;
public interface IAppointmentInterface{
    Task<Result<ReturnAppointmentDTOsimple>> CreateAppointment (CreateAppointmentDTO dto);
    Task<Result<ReturnAppointmentDTOextend>> GetAppointmentById (int id);
}