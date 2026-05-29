namespace Data.Interface;

using Api.User.Models;

public interface IUserSQL
{
    Task<(bool Success, string Role)> LoginAsync(UserModel user);
    Task<int> GetId(string cpf, string Role);
    Task<bool> EditAdressAsync(AdressModel adress);


}