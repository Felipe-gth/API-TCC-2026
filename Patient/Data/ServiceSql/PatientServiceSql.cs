using System.Runtime.InteropServices.JavaScript;
using Api.Patient.Data.InterfaceSql;
using Api.Patient.DTOs.List;
using Api.User.DTOs.Return;
using Api.User.Models;
using Dapper;

using Properties;

namespace Api.Patient.Data.ServiceSql;

public class PatientServiceSql : IPatientInterfaceSql
{
    public async Task<int> CreatePatientAsync(UserModel user)
    {
        using var connection = DBConnection.Connection();

        string hashpass = BCrypt.Net.BCrypt.HashPassword(user.Password);

        await connection.ExecuteAsync(
            @"INSERT INTO patient (name, lastName, cpf, age, password, role) VALUES (@name, @lastName, @cpf, @age, @password, @role)",
            new
            {
                name = user.Name,
                lastName = user.LastName,
                cpf = user.CPF,
                age = user.Age,
                password = hashpass,
                role = "C"
            }
        );

        int id = await connection.QuerySingleAsync<int>(
            "SELECT LAST_INSERT_ID();"
        );

        return id;
    }


    public async Task<int> CreateAddressAsync(AddressModel adress)
    {

        if (adress.IsApartment)
        {
            int id = 0;
            return id;
        }
        else
        {
            int id = 0;
            return id;
        }
    }

    public async Task<int> CreatePhoneNumberAsync(NumberModel number)
    {
        if (number.IsEmergencyContact)
        {
            int id = 0;
            return id;
        }
        else
        {
            int id = 0;
            return id;
        }
    }

    public async Task<int> CreateEmailAsync(EmailModel email)
    {
        int id = 0;
        return id;
    }

    public async Task<ListPatientDTO> GetPatientFromIdAsync(int id)
    {
        using var connection = DBConnection.Connection();

        var patient = await connection.QuerySingleOrDefaultAsync<ListPatientDTO>("SELECT id, name, lastname, age, cpf, UPPER(role) AS role FROM patient where id = @id",
            new {id = id});
        return patient;
    }
    
    public async Task<IEnumerable<ListPatientDTO>> ListAllPatient(){
        using var connection = DBConnection.Connection();
        var listpcte = await connection.QueryAsync<ListPatientDTO>("SELECT id, name, lastName, cpf, age, UPPER(role) AS role FROM patient");
        return listpcte;
    }

    public async Task<bool> EditPatientAsync(UserModel user)
    {
        using var connection = DBConnection.Connection();
        var EditPatient = connection.Execute("UPDATE patient SET name = @name, lastName = @lastName, cpf = @cpf, age = @age, password = @password WHERE id = @id",
            new {name = user.Name, lastName = user.LastName, cpf = user.CPF, age = user.Age, password = user.Password, id = user.Id});
        return EditPatient > 0;
    }

    public async Task<bool> LinkPatientToPsychologistSql (int patientId, int psychologistId)
    {
        using var connection = DBConnection.Connection();
        using var transaction = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(
                "UPDATE patient_psychologist SET active = 0 WHERE patient_id = @patientId AND active = 1;",
                new { patientId },
                transaction);

            var inserted = await connection.ExecuteAsync(
                "INSERT INTO patient_psychologist (patient_id, psychologist_id, active) VALUES (@patientId, @psychologistId, 1);",
                new { patientId, psychologistId },
                transaction);

            transaction.Commit();
            return inserted > 0;
        }
        catch
        {
            transaction.Rollback();
            return false;
        }
    }

    public async Task<ReturnPatientPsychologistDTO> GetActivePsychologistSql (int patientId)
    {
        using var connection = DBConnection.Connection();

        var result = await connection.QuerySingleOrDefaultAsync<ReturnPatientPsychologistDTO>(
            @"SELECT pp.patient_id AS PatientId,
                     pp.psychologist_id AS PsychologistId,
                     ps.name AS Name,
                     ps.lastName AS LastName,
                     ps.specialization AS Specialization
              FROM patient_psychologist pp
              JOIN psychologist ps ON ps.id = pp.psychologist_id
              WHERE pp.patient_id = @patientId
                AND pp.active = 1;",
            new { patientId });

        return result;
    }
}