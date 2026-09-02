namespace Api.Appointment.Interface;

using Api.Appointment.DTOs.Create;
using Api.Appointment.DTOs.Return;
using Api.Appointment.DTOs.Update;
using Api.Appointment.DTOs.List;
using Api.Shared.DTOs.Result;
public interface IAppointmentInterface{
    Task<Result<ReturnAppointmentDTOsimple>> CreateAppointment (CreateAppointmentDTO dto);
    Task<Result<ReturnAppointmentDTOextend>> GetAppointmentById (int id);
    Task<Result<ReturnAvailabilityDTO>> GetAvailabilityByDate (int psychologistId, DateOnly date);
    Task<Result<bool>> CreateAvailabilityDays (CreateServiceDaysDTO dto);
    Task<Result<bool>> UpdateAppointmentStatus (EntryUpdateAppointmentStatusDTO dto);
}