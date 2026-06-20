
namespace Api.Patient.Data.InterfaceSql;

using Api.Patient.Models;

public interface IAppointmentInterfaceSql
{
    Task<int> CreateAppointmentAsync(AppointmentModel appointment);
}
