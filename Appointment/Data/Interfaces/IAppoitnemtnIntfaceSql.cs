namespace Api.Appointment.Data.Interfaces;

using Api.Appointment.Models;

public interface IAppointmentInterfaceSql
{
    Task<(bool, int)> CreateAppointmentSql (AppointmentModel model);
    Task<(bool, AppointmentModel)> GetAppointmentByIdSql (int id);
}