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


        try
        {
            if(!string.IsNullOrEmpty(patient) && BCrypt.Net.BCrypt.Verify(user.Password, patient))
            {
                return (true, "C");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error verifying patient password. Hash length: {patient?.Length ?? 0}. Hash value: {patient}. Error: {ex.Message}");
        }

        try
        {
            if(!string.IsNullOrEmpty(psi) && BCrypt.Net.BCrypt.Verify(user.Password, psi))
            {
                return (true, "P");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error verifying psychologist password. Hash length: {psi?.Length ?? 0}. Hash value: {psi}. Error: {ex.Message}");
        }

        try
        {
            if(!string.IsNullOrEmpty(adm) && BCrypt.Net.BCrypt.Verify(user.Password, adm))
            {
                return (true, "A");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error verifying admin password. Hash length: {adm?.Length ?? 0}. Hash value: {adm}. Error: {ex.Message}");
        }
        
        return (false, "");
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