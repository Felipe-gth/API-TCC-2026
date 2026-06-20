namespace Api.User.Data.InterfaceSql;

using Api.User.Models;

public interface IUserSql
{
    Task<(bool Success, string Role)> LoginAsync(UserModel user);
    Task<int> GetId(string cpf, string Role);
    Task<bool> EditAddressAsync(AddressModel adress);
    Task<bool> CreateAddressAsync(AddressModel adress);
    Task<bool> CreatePhoneNumberAsync(NumberModel number);
    Task<bool> CreateEmailAsync(EmailModel email);



}