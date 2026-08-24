namespace Api.Appointment.Data.Services;

using Api.Appointment.Data.Interfaces;
using System.Data.SqlClient;
using Properties;
using Api.Appointment.Models;
using Dapper;
public class AppointmentServieSql : IAppointmentInterfaceSql
{
    public async Task<(bool, int)> CreateAppointmentSql (AppointmentModel model)
    {
        using var connection = DBConnection.Connection();

        var sql = @"INSERT INTO appointment (type, dateTime, notes, hadTreatment, physicalHealth, maritalStatus, habits, searchReason, patient_id, psychologist_id)
                     VALUES (@type, @dateTime, @notes, @hadTreatment, @physicalHealth, @maritalStatus, @habits, @searchReason, @patient_id, @psychologist_id);
                     SELECT LAST_INSERT_ID();";

        var id = await connection.QuerySingleAsync<int>(sql, new
        {
            type = model.Type,
            dateTime = model.Date,
            notes = model.Notes,
            hadTreatment = model.HadTreatment is "S" or "1" or "Y" ? 1 : 0,
            physicalHealth = string.IsNullOrEmpty(model.PhysicalHealth) ? null : model.PhysicalHealth,
            maritalStatus = string.IsNullOrEmpty(model.MaritalStatus) ? null : model.MaritalStatus,
            habits = string.IsNullOrEmpty(model.Habits) ? null : model.Habits,
            searchReason = string.IsNullOrEmpty(model.SearchReason) ? null : model.SearchReason,
            patient_id = model.PatientId,
            psychologist_id = model.PsychologistId
        });

        return (success: id > 0, id: id);
    }

    public async Task<(bool, AppointmentModel)> GetAppointmentByIdSql (int id)
    {
        using var connection = DBConnection.Connection();

        var sql = @"SELECT id, type, dateTime AS date, notes, hadTreatment, physicalHealth, maritalStatus, habits, searchReason, patient_id AS patientId, psychologist_id AS psychologistId FROM appointment WHERE id = @id";

        var appointment = await connection.QuerySingleOrDefaultAsync<AppointmentModel>(sql, new { id });

        return (success: appointment != null, appointment: appointment);
    }
}