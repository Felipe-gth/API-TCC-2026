using Api.User.Models;
using Api.User.Data.InterfaceSql;
using Dapper;
using Properties;
using BCrypt;
namespace Api.User.Data.ServicesSql;

public class UserServiceSql : IUserSql
{
    public async Task<(bool Success, string Role)> LoginAsync(UserModel user)
    {
        using var connection = DBConnection.Connection();

        var psi = connection.QueryFirstOrDefault<string>("SELECT password FROM psychologist WHERE cpf = @cpf",
            new {cpf = user.CPF});

        var adm = connection.QueryFirstOrDefault<string>("SELECT password FROM admin WHERE cpf = @cpf",
            new {cpf = user.CPF});

        var patient = connection.QueryFirstOrDefault<string>("SELECT password FROM patient WHERE cpf = @cpf",
            new {cpf = user.CPF});

        if(!string.IsNullOrEmpty(patient) && VerifyPassword(user.Password, patient))
        {
            await RehashPasswordIfLegacyAsync("patient", user.CPF, user.Password, patient);
            return (true, "C");
        }

        if(!string.IsNullOrEmpty(psi) && VerifyPassword(user.Password, psi))
        {
            await RehashPasswordIfLegacyAsync("psychologist", user.CPF, user.Password, psi);
            return (true, "P");
        }

        if(!string.IsNullOrEmpty(adm) && VerifyPassword(user.Password, adm))
        {
            await RehashPasswordIfLegacyAsync("admin", user.CPF, user.Password, adm);
            return (true, "A");
        }

        return (false, "");
    }

    private static bool VerifyPassword(string provided, string stored)
    {
        if (string.IsNullOrEmpty(stored))
            return false;

        if (stored.StartsWith("$2"))
            return BCrypt.Net.BCrypt.Verify(provided, stored);

        // Legacy plaintext fallback (seed/imported data), e.g. "senha123"
        return string.Equals(provided, stored, StringComparison.Ordinal);
    }

    private async Task RehashPasswordIfLegacyAsync(string table, string cpf, string password, string stored)
    {
        if (string.IsNullOrEmpty(stored) || stored.StartsWith("$2"))
            return;

        using var connection = DBConnection.Connection();
        var hash = BCrypt.Net.BCrypt.HashPassword(password);
        await connection.ExecuteAsync($"UPDATE {table} SET password = @hash WHERE cpf = @cpf;", new { hash, cpf });
    }
    public async Task<int> GetId(string cpf, string Role)
    {
        using var connection = DBConnection.Connection();
        if(Role == "P")
        {
            var psi = connection.QueryFirst<int>("SELECT id FROM psychologist where cpf = @cpf", 
                new {cpf = cpf});
            return psi;
        }
        else if(Role == "C")
        {
            var patient = connection.QueryFirst<int>("SELECT id FROM patient where cpf = @cpf", 
                new {cpf = cpf});
            return patient;
        }
        else
        {
            var admin = connection.QueryFirst<int>("SELECT id FROM admin where cpf = @cpf", 
                new {cpf = cpf});
            return admin; 
        }
    }

    public async Task<bool> EditAddressAsync(AddressModel adress)
    {
        using var connection = DBConnection.Connection();
        int result = 0;
        
        if (adress.IsApartment)
        {
            // Update only apartment data
            result = await connection.ExecuteAsync(
                "UPDATE address SET is_apartment = @IsApartment, floor = @Floor, apartment_number = @ApartmentNumber WHERE id = @Id",
                adress);
        }
        else
        {
            // Update only house data
            result = await connection.ExecuteAsync(
                "UPDATE address SET is_apartment = @IsApartment, street = @Street, number = @Number, neighborhood = @Neighborhood WHERE id = @Id",
                adress);
        }
        
        return result > 0;
    }

    public async Task<bool> CreateEmailAsync(EmailModel email){
        using var connection = DBConnection.Connection();
        int result = 0;
        
        return result > 0;
    }

    public async Task<bool> CreatePhoneNumberAsync(NumberModel number){
        using var connection = DBConnection.Connection();
        int result = 0;
        
        return result > 0;
    }

    public async Task<bool> CreateAddressAsync(AddressModel adress){
        using var connection = DBConnection.Connection();
        int result = 0;
        
        return result > 0;
    }
}