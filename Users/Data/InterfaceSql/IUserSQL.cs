<<<<<<< HEAD
namespace Data.Interface;

using Api.User.Models;

public interface IUserSQL
{
    Task<(bool Success, string Role)> LoginAsync(UserModel user);
    Task<int> GetId(string cpf, string Role);
    Task<bool> EditAdressAsync(AdressModel adress);
    Task<bool> CreateAdressAsync(AdressModel adress);
    Task<bool> CreatePhoneNumberAsync(NumberModel number);
    Task<bool> CreateEmailAsync(EmailModel email);



=======
namespace Data.Interface;

using Api.User.Models;

public interface IUserSQL
{
    Task<(bool Success, string Role)> LoginAsync(UserModel user);
    Task<int> GetId(string cpf, string Role);
    Task<bool> EditAdressAsync(AdressModel adress);


>>>>>>> b3e63b1 (Pull rebase)
}