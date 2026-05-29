
namespace Api.Patient.Models;

using Api.Patient.Models;
using Domain.Models.Appointment;

public interface IAppointmentInterfaceSql
{
    Task<int> CreateAppointmentAsync(AppointmentModel appointment);
}
