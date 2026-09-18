namespace Api.Admin.Data.ServiceSql;
using Api.Admin.Data.InterfaceSql;
using Api.Admin.Models;
using Dapper;
using Properties;
using Api.Admin.DTOs.Return;
using BCrypt.Net;
using System.Threading.Tasks;
using System;

public class AdminServiceSql : IAdminInterfaceSql
{
    public async Task<int> CreateAdminAsync(AdminModel admin)
    {
        using var connection = DBConnection.Connection();

        string hashpass = BCrypt.HashPassword(admin.Password);

        await connection.ExecuteAsync(
            @"INSERT INTO admin (name, lastName, cpf, password, role) VALUES (@name, @lastName, @cpf, @password, @role)",
            new
            {
                name = admin.Name,
                lastName = admin.LastName,
                cpf = admin.CPF,
                password = hashpass,
                role = "A",
            }
        );

        int id = await connection.QuerySingleAsync<int>(
            "SELECT LAST_INSERT_ID();"
        );

        return id;
    }

    public async Task<ReturnAdminData> GetAdminByIdAsync(int id)
    {
        using var connection = DBConnection.Connection();

        var adminData = await connection.QuerySingleOrDefaultAsync<ReturnAdminData>(
            @"SELECT id, name, cpf FROM admin WHERE id = @id",
            new { id }
        );

        return adminData;
    }
}