using Api.Psychologist.Data.InterfaceSql;
using Api.Psychologist.DTOs.List;
using Api.Psychologist.Models;
using Dapper;
using Properties;

namespace Api.Psychologist.Data.ServiceSql;

public class PsychologistServiceSql : IPsychologistInterfaceSql

{
    public async Task<IEnumerable<ListPsicologoDTO>> ListPsicologo()
    {
        using var connection = DBConnection.Connection();

        var list = await connection.QueryAsync<ListPsicologoDTO>(
            "SELECT id, name, lastName, cpf, crp, specialization FROM psychologist"
        );

        return list;
    }

    public async Task<int> RegisterPsicologo(PsychologistModel p)
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
            specialization = p.Espciacilization
        });

    int id = await connection.QuerySingleAsync<int>(
        "SELECT LAST_INSERT_ID();"
    );

    return id;
}

    public async Task<int> EditPsicologo(PsychologistModel p)
    {
        using var connection = DBConnection.Connection();
        string hashpass = BCrypt.Net.BCrypt.HashPassword(p.Password);
        var updated = await connection.ExecuteAsync(
            "UPDATE psychologist SET name = @name, lastName = @lastName, cpf = @cpf, age = @age, password = @password, crp = @crp, specialization = @specialization WHERE id = @id",
            new { id = p.Id, name = p.Name, lastName = p.LastName, cpf = p.CPF, age = p.Age, password = hashpass, crp = p.CRP, specialization = p.Espciacilization }
        );
        return updated;
    }
}