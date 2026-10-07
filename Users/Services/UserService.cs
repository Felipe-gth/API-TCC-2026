using Api.User.DTOs.Address;
using Api.User.Data.InterfaceSql;
using Api.User.DTOs.Return;
using Api.User.DTOs.Return.Address;
using Api.User.DTOs.Return.Phone;
using Api.User.DTOs.Return.Email;
using Api.Shared.DTOs.Result;
using Api.User.Interfaces;
using Api.User.DTOs.Login;
using Api.User.Models;
using Api.User.DTOs.Phone;
using Api.User.DTOs.Email;
using Api.User.DTOs.Delet;
using System.IdentityModel.Tokens.Jwt;
using Api.Psychologist.Data.InterfaceSql;
using Api.Admin.Data.InterfaceSql;

namespace Api.User.Services;

public class UserService : IUserInterface
{
    private readonly IUserSql _userSql;
    private readonly IAdminInterfaceSql _adminSql;
    private readonly IPsychologistInterfaceSql _psychologistSql;
    private readonly IAuthInterface _auth;

    public UserService(
        IUserSql user,
        IAuthInterface auth,
        IPsychologistInterfaceSql psychologist,
        IAdminInterfaceSql admin)
    {
        _userSql = user;
        _auth = auth;
        _psychologistSql = psychologist;
        _adminSql = admin;
    }

    public static bool TestCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf)) return false;

        cpf = new string(cpf.Where(char.IsDigit).ToArray());

        if (cpf.Length != 11) return false;
        if (cpf.Distinct().Count() == 1) return false; // rejeita 111.111.111-11 etc.

        int[] d = cpf.Select(c => (int)char.GetNumericValue(c)).ToArray();

        int CalcDigit(int count, int weightStart)
        {
            int sum = 0;
            for (int i = 0; i < count; i++)
                sum += d[i] * (weightStart - i);

            int rest = sum * 10 % 11;
            return rest == 10 ? 0 : rest;
        }

        return CalcDigit(9, 10) == d[9] && CalcDigit(10, 11) == d[10];
    }

    public async Task<Result<ReturnUserDTO>> LoginAsync(LoginUserDTO dto)
    {
        var user = new UserModel(0, "", "", dto.CPF, "0", dto.Password, "");
        var data = await _userSql.LoginAsync(user);

        if (data.Success)
        {
            int result = await _userSql.GetId(dto.CPF, data.Role);
            string token = _auth.NewToken(result, data.Role);

            var returnDTO = new ReturnUserDTO(result, token, data.Role);

            return new Result<ReturnUserDTO>()
            {
                Data = returnDTO,
                Success = true,
                Message = "Login realizado com sucesso!"
            };
        }
        else
        {
            return new Result<ReturnUserDTO>()
            {
                Data = null,
                Success = false,
                Message = "Credenciais inválidas!"
            };
        }
    }

    public async Task<Result<bool>> EditAddressAsync(AddressEntryDTO dto)
    {
        var adress = new AddressModel(dto);
        var data = await _userSql.EditAddressAsync(adress);

        if (data)
        {
            return new Result<bool>
            {
                Success = true,
                Data = true
            };
        }

        return new Result<bool>
        {
            Success = false,
            Data = false
        };
    }

    public async Task<Result<bool>> EditPhoneNumberAsync(PhoneNumberEntryDTO dto)
    {
        return new Result<bool>
        {
            Success = true,
            Data = false
        };
    }

    public async Task<Result<bool>> EditEmailAsync(EmailEntryDTO dto)
    {
        return new Result<bool>
        {
            Success = true,
            Data = false
        };
    }

    public async Task<Result<AddressReturnDTO>> CreateAddressAsync(AddressEntryDTO dto)
    {
        var adress = new AddressModel(dto);
        int addressId = await _userSql.CreateAddressAsync(adress);

        if (addressId <= 0)
        {
            return new Result<AddressReturnDTO>
            {
                Success = false,
                Data = null,
                Message = "Falha ao criar endereço"
            };
        }

        var created = new AddressReturnDTO(
            addressId,
            adress.CEP,
            adress.City,
            adress.State,
            adress.Number,
            adress.IsApartment,
            adress.ApartmentNumber
        );

        return new Result<AddressReturnDTO>
        {
            Success = true,
            Data = created,
            Message = "Endereço criado com sucesso"
        };
    }

    public async Task<Result<PhoneNumberReturnDTO>> CreatePhoneNumberAsync(PhoneNumberEntryDTO dto)
    {
        var number = new NumberModel(dto.Id, dto.Number, dto.CountryCode, dto.DDD, dto.IsEmergencyContact);
        int phoneId = await _userSql.CreatePhoneNumberAsync(number);

        if (phoneId <= 0)
        {
            return new Result<PhoneNumberReturnDTO>
            {
                Success = false,
                Data = null,
                Message = "Falha ao criar telefone"
            };
        }

        var created = new PhoneNumberReturnDTO(phoneId, number.Number, number.DDD);

        return new Result<PhoneNumberReturnDTO>
        {
            Success = true,
            Data = created,
            Message = "Telefone criado com sucesso"
        };
    }

    public async Task<Result<EmailReturnDTO>> CreateEmailAsync(EmailEntryDTO dto)
    {
        var email = new EmailModel(dto.Id, dto.Address, dto.Extension);
        int emailId = await _userSql.CreateEmailAsync(email);

        if (emailId <= 0)
        {
            return new Result<EmailReturnDTO>
            {
                Success = false,
                Data = null,
                Message = "Falha ao criar e-mail"
            };
        }

        var created = new EmailReturnDTO(emailId, email.Address, email.Extension);

        return new Result<EmailReturnDTO>
        {
            Success = true,
            Data = created,
            Message = "E-mail criado com sucesso"
        };
    }

    public async Task<Result<bool>> DeletUserAsync(DeletUserDTO dto)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(dto.Token);

        var id = jwt.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        var role = jwt.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

        bool resultado;
        string message;

        if (role.ToUpper() == "A")
        {
            var data = await _adminSql.GetAdminByIdAsync(int.Parse(id));
            int adminId = data.Id;
            string cpf = data.CPF;

            var result = await _userSql.DeletUserAsync(
                dto.UserForDeletId,
                dto.USerForDeletRole,
                adminId,
                cpf,
                dto.Password,
                "A");

            if (result)
            {
                resultado = true;
                message = $"Usuário de id {dto.UserForDeletId} deletado com sucesso";
            }
            else
            {
                resultado = false;
                message = "Erro ao deletar usuário";
            }
        }
        else
        {
            var data = await _psychologistSql.GetPsychologistById(int.Parse(id));
            int psychologistId = data.Id;
            string cpf = data.CPF;

            var result = await _userSql.DeletUserAsync(
                dto.UserForDeletId,
                dto.USerForDeletRole,
                psychologistId,
                cpf,
                dto.Password,
                "P");

            if (result)
            {
                resultado = true;
                message = $"Usuário de id {dto.UserForDeletId} deletado com sucesso";
            }
            else
            {
                resultado = false;
                message = "Erro ao deletar usuário";
            }
        }

        return new Result<bool>
        {
            Success = resultado,
            Data = resultado,
            Message = message
        };
    }

    public async Task<Result<bool>> VerifyCPFAsync(string cpf)
    {
        bool success = TestCpf(cpf);
        string message;

        if (success)
        {
            message = "Cpf válido";
        }
        else
        {
            message = "Cpf inválido";
        }

        return new Result<bool>
        {
            Success = success,
            Data = false,
            Message = message
        };
    }

    public async Task<Result<IEnumerable<AddressReturnDTO>>> GetAddressesByPatientIdAsync(int patientId)
    {
        var addresses = await _userSql.GetAddressesByPatientIdAsync(patientId);
        
        var dtos = addresses.Select(a => new AddressReturnDTO(
            a.Id,
            a.CEP,
            a.City,
            a.State,
            a.Number,
            a.IsApartment,
            a.ApartmentNumber
        ));

        return new Result<IEnumerable<AddressReturnDTO>>
        {
            Success = true,
            Data = dtos
        };
    }

    public async Task<Result<IEnumerable<PhoneNumberReturnDTO>>> GetPhoneNumbersByPatientIdAsync(int patientId)
    {
        var phones = await _userSql.GetPhoneNumbersByPatientIdAsync(patientId);
        
        var dtos = phones.Select(p => new PhoneNumberReturnDTO(
            p.Id,
            p.Number,
            p.DDD
        ));

        return new Result<IEnumerable<PhoneNumberReturnDTO>>
        {
            Success = true,
            Data = dtos
        };
    }

    public async Task<Result<IEnumerable<EmailReturnDTO>>> GetEmailsByPatientIdAsync(int patientId)
    {
        var emails = await _userSql.GetEmailsByPatientIdAsync(patientId);
        
        var dtos = emails.Select(e => new EmailReturnDTO(
            e.Id,
            e.Address,
            e.Extension
        ));

        return new Result<IEnumerable<EmailReturnDTO>>
        {
            Success = true,
            Data = dtos
        };
    }

    // Update methods
    public async Task<Result<AddressReturnDTO>> UpdateAddressAsync(int addressId, AddressEntryDTO dto)
    {
        var address = new AddressModel(dto);
        address.Id = addressId; // Override with route parameter ID

        bool success = await _userSql.UpdateAddressAsync(address);

        if (!success)
        {
            return new Result<AddressReturnDTO>
            {
                Success = false,
                Data = null,
                Message = "Endereço não encontrado ou falha ao atualizar"
            };
        }

        var updated = new AddressReturnDTO(
            addressId,
            address.CEP,
            address.City,
            address.State,
            address.Number,
            address.IsApartment,
            address.ApartmentNumber
        );

        return new Result<AddressReturnDTO>
        {
            Success = true,
            Data = updated,
            Message = "Endereço atualizado com sucesso"
        };
    }

    public async Task<Result<PhoneNumberReturnDTO>> UpdatePhoneNumberAsync(int phoneId, PhoneNumberEntryDTO dto)
    {
        var number = new NumberModel(phoneId, dto.Number, dto.CountryCode, dto.DDD, dto.IsEmergencyContact);

        bool success = await _userSql.UpdatePhoneNumberAsync(number);

        if (!success)
        {
            return new Result<PhoneNumberReturnDTO>
            {
                Success = false,
                Data = null,
                Message = "Telefone não encontrado ou falha ao atualizar"
            };
        }

        var updated = new PhoneNumberReturnDTO(phoneId, number.Number, number.DDD);

        return new Result<PhoneNumberReturnDTO>
        {
            Success = true,
            Data = updated,
            Message = "Telefone atualizado com sucesso"
        };
    }

    public async Task<Result<EmailReturnDTO>> UpdateEmailAsync(int emailId, EmailEntryDTO dto)
    {
        var email = new EmailModel(emailId, dto.Address, dto.Extension);

        bool success = await _userSql.UpdateEmailAsync(email);

        if (!success)
        {
            return new Result<EmailReturnDTO>
            {
                Success = false,
                Data = null,
                Message = "E-mail não encontrado ou falha ao atualizar"
            };
        }

        var updated = new EmailReturnDTO(emailId, email.Address, email.Extension);

        return new Result<EmailReturnDTO>
        {
            Success = true,
            Data = updated,
            Message = "E-mail atualizado com sucesso"
        };
    }

    // Delete methods
    public async Task<Result<bool>> DeleteAddressAsync(int addressId)
    {
        bool success = await _userSql.DeleteAddressAsync(addressId);

        return new Result<bool>
        {
            Success = success,
            Data = success,
            Message = success ? "Endereço deletado com sucesso" : "Endereço não encontrado"
        };
    }

    public async Task<Result<bool>> DeletePhoneNumberAsync(int phoneId)
    {
        bool success = await _userSql.DeletePhoneNumberAsync(phoneId);

        return new Result<bool>
        {
            Success = success,
            Data = success,
            Message = success ? "Telefone deletado com sucesso" : "Telefone não encontrado"
        };
    }

    public async Task<Result<bool>> DeleteEmailAsync(int emailId)
    {
        bool success = await _userSql.DeleteEmailAsync(emailId);

        return new Result<bool>
        {
            Success = success,
            Data = success,
            Message = success ? "E-mail deletado com sucesso" : "E-mail não encontrado"
        };
    }
}