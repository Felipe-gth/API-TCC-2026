using Api.Psychologist.Data.InterfaceSql;
using Api.Psychologist.DTOs.List;
using Api.Psychologist.Models;
using Dapper;
using Properties;

namespace Api.Psychologist.Data.ServiceSql;

public class PsychologistServiceSql : IPsychologistInterfaceSql

{
    public async Task<IEnumerable<ListPsychologistDTO>> ListPsychologist()
    {
        using var connection = DBConnection.Connection();

        var list = await connection.QueryAsync<ListPsychologistDTO>(
            "SELECT id, name, lastName, cpf, crp, specialization FROM psychologist"
        );

        return list;
    }

    public async Task<int> RegisterPsychologist(PsychologistModel p)
{
    using var connection = DBConnection.Connection();

    string hashpass = BCrypt.Net.BCrypt.HashPassword(p.Password);

    await connection.ExecuteAsync(
        @"INSERT INTO psychologist 
        (name, lastName, cpf, age, password, role, crp, specialization) 
        VALUES 
        (@name, @lastName, @cpf, @age, @password, @role, @crp, @specialization)",
        new
        {
            name = p.Name,
            lastName = p.LastName,
            cpf = p.CPF,
            age = p.Age,
            password = hashpass,
            role = "P",
            crp = p.CRP,
            specialization = p.Specialization
        });

    int id = await connection.QuerySingleAsync<int>(
        "SELECT LAST_INSERT_ID();"
    );

    return id;
}

    public async Task<int> EditPsychologist(PsychologistModel p)
    {
        using var connection = DBConnection.Connection();
        string hashpass = BCrypt.Net.BCrypt.HashPassword(p.Password);
        var updated = await connection.ExecuteAsync(
            "UPDATE psychologist SET name = @name, lastName = @lastName, cpf = @cpf, age = @age, password = @password, crp = @crp, specialization = @specialization WHERE id = @id",
            new { id = p.Id, name = p.Name, lastName = p.LastName, cpf = p.CPF, age = p.Age, password = hashpass, crp = p.CRP, specialization = p.Specialization }
        );
        return updated;
    }

    public async Task<ListPsychologistDTO> GetPsychologistById(int id)
    {
        using var connection = DBConnection.Connection();

        var result = await connection.QuerySingleOrDefaultAsync<ListPsychologistDTO>(
            "SELECT id, name, lastName, cpf, crp, specialization FROM psychologist WHERE id = @id",
            new { id });

        return result;
    }

    public async Task<bool> DeletePsychologist(int id)
    {
        using var connection = DBConnection.Connection();
        using var transaction = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(
                "DELETE FROM patient_psychologist WHERE psychologist_id = @id;",
                new { id }, transaction);
            await connection.ExecuteAsync(
                "DELETE FROM psychologist_address WHERE psychologist_id = @id;",
                new { id }, transaction);
            await connection.ExecuteAsync(
                "DELETE FROM psychologist_email WHERE psychologist_id = @id;",
                new { id }, transaction);
            await connection.ExecuteAsync(
                "DELETE FROM psychologist_phone_number WHERE psychologist_id = @id;",
                new { id }, transaction);
            await connection.ExecuteAsync(
                "DELETE FROM avaliable_time WHERE serviceDay_id IN (SELECT id FROM service_days WHERE psychologist_id = @id);",
                new { id }, transaction);
            await connection.ExecuteAsync(
                "DELETE FROM service_days WHERE psychologist_id = @id;",
                new { id }, transaction);

            var rows = await connection.ExecuteAsync(
                "DELETE FROM psychologist WHERE id = @id;",
                new { id }, transaction);

            transaction.Commit();
            return rows > 0;
        }
        catch
        {
            transaction.Rollback();
            return false;
        }
    }
}