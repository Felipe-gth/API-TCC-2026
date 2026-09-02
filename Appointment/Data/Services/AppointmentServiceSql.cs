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

        var sql = @"INSERT INTO appointment (type, dateTime, notes, hadTreatment, physicalHealth, maritalStatus, habits, searchReason, status, patient_id, psychologist_id)
                     VALUES (@type, @dateTime, @notes, @hadTreatment, @physicalHealth, @maritalStatus, @habits, @searchReason, 'pendente', @patient_id, @psychologist_id);
                     SELECT LAST_INSERT_ID();";

        var id = await connection.QuerySingleAsync<int>(sql, new
        {
            type = model.Type,
            dateTime = model.Date,
            notes = model.Notes ?? "",
            hadTreatment = model.HadTreatment is "S" or "1" or "Y" ? 1 : 0,
            physicalHealth = string.IsNullOrEmpty(model.PhysicalHealth) ? "" : model.PhysicalHealth,
            maritalStatus = string.IsNullOrEmpty(model.MaritalStatus) ? "" : model.MaritalStatus,
            habits = string.IsNullOrEmpty(model.Habits) ? "" : model.Habits,
            searchReason = string.IsNullOrEmpty(model.SearchReason) ? "" : model.SearchReason,
            patient_id = model.PatientId,
            psychologist_id = model.PsychologistId
        });

        return (success: id > 0, id: id);
    }

    public async Task<(bool, AppointmentModel)> GetAppointmentByIdSql (int id)
    {
        using var connection = DBConnection.Connection();

        var sql = @"SELECT id, type, dateTime AS date, notes, hadTreatment, physicalHealth, maritalStatus, habits, searchReason, status, patient_id AS patientId, psychologist_id AS psychologistId FROM appointment WHERE id = @id";

        var appointment = await connection.QuerySingleOrDefaultAsync<AppointmentModel>(sql, new { id });

        return (success: appointment != null, appointment: appointment);
    }

    public async Task<(bool, bool)> UpdateAppointmentStatusSql (int id, string status)
    {
        using var connection = DBConnection.Connection();

        var sql = @"UPDATE appointment SET status = @status WHERE id = @id;";

        var rows = await connection.ExecuteAsync(sql, new { id, status });

        return (success: rows > 0, updated: rows > 0);
    }

    public async Task<IEnumerable<TimeSpan>> GetTemplateHours (int psychologistId, int weekDay)
    {
        using var connection = DBConnection.Connection();

        var sql = @"SELECT at.hours
                    FROM avaliable_time at
                    JOIN service_days sd ON sd.id = at.serviceDay_id
                    WHERE sd.psychologist_id = @psychologistId
                      AND sd.week_days = @weekDay
                      AND at.status != 'bloqueado'
                    ORDER BY at.hours;";

        return await connection.QueryAsync<TimeSpan>(sql, new { psychologistId, weekDay });
    }

    public async Task<HashSet<TimeSpan>> GetTakenHours (int psychologistId, DateTime start, DateTime end)
    {
        using var connection = DBConnection.Connection();

        var sql = @"SELECT TIME(dateTime)
                    FROM appointment
                    WHERE psychologist_id = @psychologistId
                      AND dateTime >= @start
                      AND dateTime <  @end;";

        var rows = await connection.QueryAsync<TimeSpan>(sql, new { psychologistId, start, end });
        return rows.ToHashSet();
    }

    public async Task<(bool success, bool registered)> RegisterServiceDaysSql (int psychologistId, List<(int weekDay, string hour)> slots)
    {
        using var connection = DBConnection.Connection();
        using var transaction = connection.BeginTransaction();

        try
        {
            foreach (var group in slots.GroupBy(s => s.weekDay))
            {
                var serviceDayId = await connection.QuerySingleAsync<int>(
                    "INSERT INTO service_days (psychologist_id, week_days) VALUES (@psychologistId, @weekDay); SELECT LAST_INSERT_ID();",
                    new { psychologistId, weekDay = group.Key },
                    transaction);

                foreach (var slot in group)
                {
                    await connection.ExecuteAsync(
                        "INSERT INTO avaliable_time (serviceDay_id, hours, status) VALUES (@serviceDayId, @hour, 'disponivel');",
                        new { serviceDayId, hour = slot.hour },
                        transaction);
                }
            }

            transaction.Commit();
            return (success: true, registered: true);
        }
        catch
        {
            transaction.Rollback();
            return (success: false, registered: false);
        }
    }

    public async Task<(bool, string)> ValidateAppointmentSlotSql (int psychologistId, DateTime dateTime)
    {
        using var connection = DBConnection.Connection();

        var weekDay = (int)dateTime.DayOfWeek;
        var hour = dateTime.ToString(@"HH\:mm");

        var notBlocked = await connection.QuerySingleOrDefaultAsync<bool?>(
            @"SELECT at.status != 'bloqueado'
              FROM avaliable_time at
              JOIN service_days sd ON sd.id = at.serviceDay_id
              WHERE sd.psychologist_id = @psychologistId
                AND sd.week_days = @weekDay
                AND at.hours = @hour;",
            new { psychologistId, weekDay, hour });

        if (notBlocked is null)
            return (available: false, reason: "grade");
        if (notBlocked == false)
            return (available: false, reason: "blocked");

        var end = dateTime.AddMinutes(1);
        var conflict = await connection.ExecuteScalarAsync<int>(
            @"SELECT COUNT(*)
              FROM appointment
              WHERE psychologist_id = @psychologistId
                AND dateTime >= @dateTime
                AND dateTime <  @end;",
            new { psychologistId, dateTime, end });

        if (conflict > 0)
            return (available: false, reason: "occupied");

        return (available: true, reason: "");
    }
}