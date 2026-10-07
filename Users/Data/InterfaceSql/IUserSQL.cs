namespace Api.User.Data.InterfaceSql;

using Api.User.Models;

public interface IUserSql
{
    Task<(bool Success, string Role)> LoginAsync(UserModel user);
    Task<int> GetId(string cpf, string Role);
    Task<bool> EditAddressAsync(AddressModel adress);
    Task<int> CreateAddressAsync(AddressModel adress);
    Task<int> CreatePhoneNumberAsync(NumberModel number);
    Task<int> CreateEmailAsync(EmailModel email);
    Task<bool> DeletUserAsync(int userForDeletId, string userForDeletROle, int userRequieredId, string cpf, string password, string role);

    Task<IEnumerable<AddressModel>> GetAddressesByPatientIdAsync(int patientId);
    Task<IEnumerable<NumberModel>> GetPhoneNumbersByPatientIdAsync(int patientId);
    Task<IEnumerable<EmailModel>> GetEmailsByPatientIdAsync(int patientId);

    // Update methods
    Task<bool> UpdateAddressAsync(AddressModel address);
    Task<bool> UpdatePhoneNumberAsync(NumberModel number);
    Task<bool> UpdateEmailAsync(EmailModel email);

    // Delete methods
    Task<bool> DeleteAddressAsync(int addressId);
    Task<bool> DeletePhoneNumberAsync(int phoneId);
    Task<bool> DeleteEmailAsync(int emailId);
}