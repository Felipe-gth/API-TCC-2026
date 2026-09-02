namespace Api.Appointment.Services;


using Api.Appointment.Interface;
using Api.Appointment.Data.Interfaces;
using Api.Appointment.Models;
using Api.Shared.DTOs.Result;
using Api.Appointment.DTOs.Create;
using Api.Appointment.DTOs.Return;
using Api.Appointment.DTOs.List;
using Api.Appointment.DTOs.Update;
using Api.Appointment.DTOs;

public class AppointmentService : IAppointmentInterface
{
    private readonly IAppointmentInterfaceSql _appointmentSql;
    public AppointmentService(IAppointmentInterfaceSql appointmentSql)
    {
        _appointmentSql = appointmentSql;
    }

    public async Task<Result<ReturnAppointmentDTOsimple>> CreateAppointment (CreateAppointmentDTO dto)
    {
        var (available, reason) = await _appointmentSql.ValidateAppointmentSlotSql(dto.PsychologistId, dto.DateAndTime);
        if (!available)
            return new Result<ReturnAppointmentDTOsimple>(false, null);

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

    public async Task<Result<bool>> CreateAvailabilityDays(CreateServiceDaysDTO dto)
    {
        if (dto == null || dto.Days == null || dto.Days.Count == 0)
            return new Result<bool>(false, false);

        var slots = dto.Days
            .SelectMany(d => d.Hours.Select(h => (d.WeekDay, h)))
            .ToList();

        if (slots.Count == 0)
            return new Result<bool>(false, false);

        var (success, registered) = await _appointmentSql.RegisterServiceDaysSql(dto.PsychologistId, slots);
        return new Result<bool>(success, registered);
    }

    public async Task<Result<ReturnAvailabilityDTO>> GetAvailabilityByDate (int psychologistId, DateOnly date)
    {
        var weekDay = (int)date.DayOfWeek;
        var start = date.ToDateTime(TimeOnly.MinValue);
        var end = start.AddDays(1);

        var template = (await _appointmentSql.GetTemplateHours(psychologistId, weekDay)).ToHashSet();
        if (template.Count == 0)
            return new Result<ReturnAvailabilityDTO>(true, null);

        var taken = await _appointmentSql.GetTakenHours(psychologistId, start, end);

        var dto = new ReturnAvailabilityDTO
        {
            PsychologistId = psychologistId,
            Date = date,
            WeekDay = weekDay,
            Avaliability = true,
            Hours = template
                .OrderBy(h => h)
                .Select(h => new HourAvailabilityDTO
                {
                    Hour = h.ToString(@"hh\:mm"),
                    Available = !taken.Contains(h)
                })
                .ToList()
        };
        return new Result<ReturnAvailabilityDTO>(true, dto);
    }

    public async Task<Result<bool>> UpdateAppointmentStatus (EntryUpdateAppointmentStatusDTO dto)
    {
        var status = dto.Status switch
        {
            AppointmentStatus.Finalizado => "finalizado",
            AppointmentStatus.Cancelado => "cancelado",
            _ => "pendente"
        };

        var (success, updated) = await _appointmentSql.UpdateAppointmentStatusSql(dto.Id, status);
        return new Result<bool>(success, updated);
    }
} 