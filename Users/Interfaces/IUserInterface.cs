using Api.User.DTOs.Login;
using Api.User.DTOs.Return;
using Api.User.DTOs.Return.Address;
using Api.User.DTOs.Return.Phone;
using Api.User.DTOs.Return.Email;
using Api.Shared.DTOs.Result;
using Api.User.DTOs.Address;
using Api.User.DTOs.Phone;
using Api.User.DTOs.Email;
using Api.User.DTOs.Delet;

namespace Api.User.Interfaces;

public interface IUserInterface
{
    Task<Result<ReturnUserDTO>> LoginAsync(LoginUserDTO dto);
    Task<Result<bool>> EditAddressAsync(AddressEntryDTO dto);
    Task<Result<bool>> EditPhoneNumberAsync(PhoneNumberEntryDTO dto);
    Task<Result<bool>> EditEmailAsync(EmailEntryDTO dto);
    Task<Result<AddressReturnDTO>> CreateAddressAsync(AddressEntryDTO dto);
    Task<Result<PhoneNumberReturnDTO>> CreatePhoneNumberAsync(PhoneNumberEntryDTO dto);
    Task<Result<EmailReturnDTO>> CreateEmailAsync(EmailEntryDTO dto);
    Task<Result<bool>> DeletUserAsync(DeletUserDTO dto);
    Task<Result<bool>> VerifyCPFAsync(string cpf);

    // Get methods for listing
    Task<Result<IEnumerable<AddressReturnDTO>>> GetAddressesByPatientIdAsync(int patientId);
    Task<Result<IEnumerable<PhoneNumberReturnDTO>>> GetPhoneNumbersByPatientIdAsync(int patientId);
    Task<Result<IEnumerable<EmailReturnDTO>>> GetEmailsByPatientIdAsync(int patientId);

    // Update methods
    Task<Result<AddressReturnDTO>> UpdateAddressAsync(int addressId, AddressEntryDTO dto);
    Task<Result<PhoneNumberReturnDTO>> UpdatePhoneNumberAsync(int phoneId, PhoneNumberEntryDTO dto);
    Task<Result<EmailReturnDTO>> UpdateEmailAsync(int emailId, EmailEntryDTO dto);

    // Delete methods
    Task<Result<bool>> DeleteAddressAsync(int addressId);
    Task<Result<bool>> DeletePhoneNumberAsync(int phoneId);
    Task<Result<bool>> DeleteEmailAsync(int emailId);
}