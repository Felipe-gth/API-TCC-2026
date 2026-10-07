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
                "UPDATE patient_address SET is_apartment = @IsApartment, floor = @Floor, apartment_number = @ApartmentNumber WHERE id = @Id",
                adress);
        }
        else
        {
            // Update only house data
            result = await connection.ExecuteAsync(
                "UPDATE patient_address SET is_apartment = @IsApartment, street = @Street, number = @Number, neighborhood = @Neighborhood, cep = @CEP, city = @City, state = @State WHERE id = @Id",
                adress);
        }
        
        return result > 0;
    }

    public async Task<int> CreateAddressAsync(AddressModel adress)
    {
        using var connection = DBConnection.Connection();
        
        const string sql = @"
            INSERT INTO patient_address 
            (cep, city, state, street, number, neighborhood, is_apartment, floor, apartment_number, patient_id)
            VALUES 
            (@CEP, @City, @State, @Street, @Number, @Neighborhood, @IsApartment, @Floor, @ApartmentNumber, @PatientId);
            SELECT LAST_INSERT_ID();";

        int id = await connection.QuerySingleAsync<int>(sql, adress);
        
        return id;
    }

    public async Task<int> CreatePhoneNumberAsync(NumberModel number)
    {
        using var connection = DBConnection.Connection();
        
        const string sql = @"
            INSERT INTO patient_phone_number 
            (number, country_code, ddd, is_emergency, patient_id)
            VALUES 
            (@Number, @CountryCode, @DDD, @IsEmergencyContact, @Id);
            SELECT LAST_INSERT_ID();";

        int id = await connection.QuerySingleAsync<int>(sql, number);
        
        return id;
    }

    public async Task<int> CreateEmailAsync(EmailModel email)
    {
        using var connection = DBConnection.Connection();
        
        const string sql = @"
            INSERT INTO patient_email 
            (address, extension, patient_id)
            VALUES 
            (@Address, @Extension, @Id);
            SELECT LAST_INSERT_ID();";

        int id = await connection.QuerySingleAsync<int>(sql, email);
        
        return id;
    }

    public async Task<bool> DeletUserAsync(int userForDeletId,string userForDeletROle, int userRequieredId, string cpf, string password, string role)
    {   
        using var connection = DBConnection.Connection();
        var (success, userRole) = await LoginAsync(new UserModel { CPF = cpf, Password = password.ToString() });
        string Person;
        if (success)
    {
        if(userForDeletROle == "P"){
            Person = "patient";
        }
        else{
            Person = "psychologist";
        }

        string sql = $"DELETE FROM {Person} WHERE id = @Id";

        int rowsAffected = await connection.ExecuteAsync(
            sql,
            new { Id = userForDeletId }
        );

        return rowsAffected > 0;
    }
        return false;
    }

    public async Task<IEnumerable<AddressModel>> GetAddressesByPatientIdAsync(int patientId)
    {
        using var connection = DBConnection.Connection();
        
        const string sql = @"
            SELECT id, cep, city, state, street, number, neighborhood, is_apartment, floor, apartment_number, patient_id
            FROM patient_address
            WHERE patient_id = @patientId;";

        return await connection.QueryAsync<AddressModel>(sql, new { patientId });
    }

    public async Task<IEnumerable<NumberModel>> GetPhoneNumbersByPatientIdAsync(int patientId)
    {
        using var connection = DBConnection.Connection();
        
        const string sql = @"
            SELECT id, number, country_code as CountryCode, ddd, is_emergency as IsEmergencyContact
            FROM patient_phone_number
            WHERE patient_id = @patientId;";

        return await connection.QueryAsync<NumberModel>(sql, new { patientId });
    }

    public async Task<IEnumerable<EmailModel>> GetEmailsByPatientIdAsync(int patientId)
    {
        using var connection = DBConnection.Connection();
        
        const string sql = @"
            SELECT id, address, extension
            FROM patient_email
            WHERE patient_id = @patientId;";

        return await connection.QueryAsync<EmailModel>(sql, new { patientId });
    }

    // Update methods
    public async Task<bool> UpdateAddressAsync(AddressModel address)
    {
        using var connection = DBConnection.Connection();
        
        const string sql = @"
            UPDATE patient_address 
            SET cep = @CEP, city = @City, state = @State, street = @Street, 
                number = @Number, neighborhood = @Neighborhood, 
                is_apartment = @IsApartment, floor = @Floor, apartment_number = @ApartmentNumber
            WHERE id = @Id;";

        int result = await connection.ExecuteAsync(sql, address);
        return result > 0;
    }

    public async Task<bool> UpdatePhoneNumberAsync(NumberModel number)
    {
        using var connection = DBConnection.Connection();
        
        const string sql = @"
            UPDATE patient_phone_number 
            SET number = @Number, country_code = @CountryCode, ddd = @DDD, is_emergency = @IsEmergencyContact
            WHERE id = @Id;";

        int result = await connection.ExecuteAsync(sql, number);
        return result > 0;
    }

    public async Task<bool> UpdateEmailAsync(EmailModel email)
    {
        using var connection = DBConnection.Connection();
        
        const string sql = @"
            UPDATE patient_email 
            SET address = @Address, extension = @Extension
            WHERE id = @Id;";

        int result = await connection.ExecuteAsync(sql, email);
        return result > 0;
    }

    // Delete methods
    public async Task<bool> DeleteAddressAsync(int addressId)
    {
        using var connection = DBConnection.Connection();
        
        const string sql = "DELETE FROM patient_address WHERE id = @Id;";
        
        int result = await connection.ExecuteAsync(sql, new { Id = addressId });
        return result > 0;
    }

    public async Task<bool> DeletePhoneNumberAsync(int phoneId)
    {
        using var connection = DBConnection.Connection();
        
        const string sql = "DELETE FROM patient_phone_number WHERE id = @Id;";
        
        int result = await connection.ExecuteAsync(sql, new { Id = phoneId });
        return result > 0;
    }

    public async Task<bool> DeleteEmailAsync(int emailId)
    {
        using var connection = DBConnection.Connection();
        
        const string sql = "DELETE FROM patient_email WHERE id = @Id;";
        
        int result = await connection.ExecuteAsync(sql, new { Id = emailId });
        return result > 0;
    }
}