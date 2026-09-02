namespace Api.Appointment.Data.Interfaces;

using Api.Appointment.Models;

public interface IAppointmentInterfaceSql
{
    Task<(bool, int)> CreateAppointmentSql (AppointmentModel model);
    Task<(bool, AppointmentModel)> GetAppointmentByIdSql (int id);
    Task<IEnumerable<TimeSpan>> GetTemplateHours (int psychologistId, int weekDay);
    Task<HashSet<TimeSpan>> GetTakenHours (int psychologistId, DateTime start, DateTime end);
    Task<(bool success, bool registered)> RegisterServiceDaysSql (int psychologistId, List<(int weekDay, string hour)> slots);
    Task<(bool success, bool updated)> UpdateAppointmentStatusSql (int id, string status);
    Task<(bool available, string reason)> ValidateAppointmentSlotSql (int psychologistId, DateTime dateTime);
}