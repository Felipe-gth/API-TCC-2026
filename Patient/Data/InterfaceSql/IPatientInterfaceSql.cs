
using System.Runtime.InteropServices.JavaScript;
using Api.User.DTOs.Return;

namespace Api.Patient.Data.InterfaceSql;

using Api.User.Models;
using Api.Patient.DTOs.List;

public interface IPatientInterfaceSql
{
    Task<int> CreatePatientAsync(UserModel user);
    Task<int> CreateAdressAsync(AdressModel adress);
    Task<int> CreatePhoneNumberAsync(NumberModel number);
    Task<int> CreateEmailAsync(EmailModel email);
    Task<ReturnUserDTO> GetPatientFromIdAsync(int id);
    Task<IEnumerable<ListPatientDTO>> ListAllPatient();
    Task<bool> EditPatientAsync(UserModel user);

}
