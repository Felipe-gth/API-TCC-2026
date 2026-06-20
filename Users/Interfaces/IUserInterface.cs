
using Api.User.DTOs.Address;

namespace Api.User.Interfaces;

using Api.User.DTOs.Login;
using Api.User.DTOs.Return;
using Api.Shared.DTOs.Result;
using Api.User.DTOs.Address;
using Api.User.DTOs.Phone;
using Api.User.DTOs.Email;


public interface IUserInterface
{
    Task<Result<ReturnUserDTO>> LoginAsync(LoginUserDTO dto);
    Task<bool> EditAddressAsync(AddressEntryDTO dto);
    Task<Result<bool>> CreateAddressAsync(AddressEntryDTO dto);
    Task<Result<bool>> CreatePhoneNumberAsync(PhoneNumberEntryDTO dto);
    Task<Result<bool>> CreateEmailAsync(EmailEntryDTO dto);
}