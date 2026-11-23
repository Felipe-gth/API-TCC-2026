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
            "SELECT id, name, lastName, cpf, age, crp, specialization, isActive FROM psychologist WHERE isActive = true"
        );

        return list;
    }

    public async Task<IEnumerable<ListPsychologistDTO>> ListPsychologistNotActive()
    {
        using var connection = DBConnection.Connection();

        var list = await connection.QueryAsync<ListPsychologistDTO>(
            "SELECT id, name, lastName, cpf, age, crp, specialization, isActive FROM psychologist WHERE isActive = false"
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

        // Senha em branco: mantém a senha atual do psicólogo.
        if (string.IsNullOrWhiteSpace(p.Password))
        {
            var updatedWithoutPassword = await connection.ExecuteAsync(
                @"UPDATE psychologist
                  SET name = @name, lastName = @lastName, cpf = @cpf, age = @age,
                      crp = @crp, specialization = @specialization
                  WHERE id = @id AND isActive = true",
                new { id = p.Id, name = p.Name, lastName = p.LastName, cpf = p.CPF, age = p.Age, crp = p.CRP, specialization = p.Specialization }
            );
            return updatedWithoutPassword;
        }

        string hashpass = BCrypt.Net.BCrypt.HashPassword(p.Password);

        var updated = await connection.ExecuteAsync(
            @"UPDATE psychologist
              SET name = @name, lastName = @lastName, cpf = @cpf, age = @age,
                  crp = @crp, specialization = @specialization, password = @password
              WHERE id = @id AND isActive = true",
            new { id = p.Id, name = p.Name, lastName = p.LastName, cpf = p.CPF, age = p.Age, crp = p.CRP, specialization = p.Specialization, password = hashpass }
        );
        return updated;
    }

    public async Task<ListPsychologistDTO> GetPsychologistById(int id)
    {
        using var connection = DBConnection.Connection();

        var result = await connection.QuerySingleOrDefaultAsync<ListPsychologistDTO>(
            "SELECT id, name, lastName, cpf, age, crp, specialization, isActive FROM psychologist WHERE id = @id AND isActive = true",
            new { id });

        return result;
    }

    public async Task<int> DeletePsychologist(int id)
    {
        using var connection = DBConnection.Connection();

        var updated = await connection.ExecuteAsync(
            "UPDATE psychologist SET isActive = false WHERE id = @id AND isActive = true",
            new { id }
        );

        return updated;
    }

    public async Task<int> ReactivatePsychologist(int id)
    {
        using var connection = DBConnection.Connection();

        var updated = await connection.ExecuteAsync(
            "UPDATE psychologist SET isActive = true WHERE id = @id AND isActive = false",
            new { id }
        );

        return updated;
    }
}