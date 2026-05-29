
using Api.User.DTOs.Adress;

namespace Api.User.Interfaces;

using Api.User.DTOs.Login;
using Api.User.DTOs.Return;
using Api.User.DTOs.Result;


public interface IUserInterface
{
    Task<Result<ReturnUserDTO>> LoginAsync(LoginUserDTO dto);
    Task<bool> EditAdressAsync(AdressEntryDTO dto);
}