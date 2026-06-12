
using Api.User.DTOs.Adress;

namespace Api.User.Interfaces;

using Api.User.DTOs.Login;
using Api.User.DTOs.Return;
using Api.User.DTOs.Result;
using Api.User.DTOs.Adress;
using Api.User.DTOs.Phone;
using Api.User.DTOs.Email;


public interface IUserInterface
{
    Task<Result<ReturnUserDTO>> LoginAsync(LoginUserDTO dto);
    Task<bool> EditAdressAsync(AdressEntryDTO dto);
    Task<Result<bool>> CreateAdressAsync(AdressEntryDTO dto);
    Task<Result<bool>> CreatePhoneNumberAsync(PhoneNumberEntryDTO dto);
    Task<Result<bool>> CreateEmailAsync(EmailEntryDTO dto);
}