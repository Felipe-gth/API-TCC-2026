namespace Api.Appointment.Data.Services;

using Api.Appointment.Data.Interfaces;
using Api.Appointment.Models;

public class AppointmentServieSql : IAppointmentInterfaceSql
{
    public async Task<(bool, int)> CreateAppointmentSql (AppointmentModel model)
    {
        return (success: true, id: 0);
    }
}