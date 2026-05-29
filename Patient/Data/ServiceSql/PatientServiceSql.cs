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


    public async Task<int> CreateAdressAsync(AdressModel adress)
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

    public async Task<ReturnUserDTO> GetPatientFromIdAsync(int id)
    {
        using var connection = DBConnection.Connection();

        var patient = connection.QueryFirst<ReturnUserDTO>("SELECT name, lastname, age, cpf, role FROM patient where id = @id",
            new {id = id});
        if (patient != null)
        {
            return patient;
        }
        return null;
    }
    
    public async Task<IEnumerable<ListPatientDTO>> ListAllPatient(){
        var connection = DBConnection.Connection();
        var listpcte = connection.Query<ListPatientDTO>("SELECT name, lastName, cpf, age, role FROM patient");
        //var listpcte = connection.Query<ListPatientDTO>("SELECT p.Name, p.lastName, p.cpf, ppn.number, ppn.countrycode, ppn.ddd FROM patient p INNER JOIN patient_phone_number ppn ON p.id = ppn.patient_id");
        return listpcte;
    }

    public async Task<bool> EditPatientAsync(UserModel user)
    {
        using var connection = DBConnection.Connection();
        var EditPatient = connection.Execute("UPDATE patient SET name = @name, lastName = @lastName, cpf = @cpf, age = @age, password = @password WHERE id = @id",
            new {name = user.Name, lastName = user.LastName, cpf = user.CPF, age = user.Age, password = user.Password, id = user.Id});
        return EditPatient > 0;
    }
}