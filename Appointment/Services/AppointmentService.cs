namespace Api.Appointment.Services;


using Api.Appointment.Interface;
using Api.Appointment.Data.Interfaces;
using Api.Appointment.Models;
using Api.Shared.DTOs.Result;
using Api.Appointment.DTOs.Create;
using Api.Appointment.DTOs.Return;

public class AppointmentService : IAppointmentInterface
{
    private readonly IAppointmentInterfaceSql _appointmentSql;
    public AppointmentService(IAppointmentInterfaceSql appointmentSql)
    {
        _appointmentSql = appointmentSql;
    }

    public async Task<Result<ReturnAppointmentDTOsimple>> CreateAppointment (CreateAppointmentDTO dto)
    {
        var model = new AppointmentModel(dto.Type, dto.DateAndTime, dto.Notes, dto.MaritalStatus, dto.PhysicalHealth, dto.HadTreatment, dto.Habits, dto.SearchReason, dto.PatientId, dto.PsychologistId);
        var (success, id) = await _appointmentSql.CreateAppointmentSql(model);
        var type = model.Type switch
        {
            "O" => "Online",
            "P" => "Presencial",
            _ => "P"
        };

        if (success)
        {
            var appointmentReturn = new ReturnAppointmentDTOsimple(id, type, model.Date.Day, model.Date.Month, model.Date.Hour, model.PatientId, model.PsychologistId);
            var resultReturn = new Result<ReturnAppointmentDTOsimple>(true, appointmentReturn);
            return resultReturn;
        }
        var resultReturnError = new Result<ReturnAppointmentDTOsimple>(false, null);
        return resultReturnError;
    }

    public async Task<Result<ReturnAppointmentDTOextend>> GetAppointmentById (int id)
    {
        var (success, appointment) = await _appointmentSql.GetAppointmentByIdSql(id);
        if (success && appointment != null)
        {
            var appointmentReturn = new ReturnAppointmentDTOextend(appointment.Id, appointment.Type, appointment.Date.Day, appointment.Date.Month, appointment.Date.Hour, appointment.Notes, appointment.MaritalStatus, appointment.PhysicalHealth, appointment.HadTreatment, appointment.Habits, appointment.SearchReason, appointment.PatientId, appointment.PsychologistId);
            var resultReturn = new Result<ReturnAppointmentDTOextend>(true, appointmentReturn);
            return resultReturn;
        }
        var resultReturnError = new Result<ReturnAppointmentDTOextend>(false, null);
        return resultReturnError;
    }
}
