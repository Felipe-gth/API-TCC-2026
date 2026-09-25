
using Api.User.DTOs.Address;
using Api.User.Data.InterfaceSql;
using Api.User.DTOs.Return;
using Api.Shared.DTOs.Result;
using Api.User.Interfaces;
using Api.User.DTOs.Login;
using Api.User.Models;
using Api.User.DTOs.Phone;
using Api.User.DTOs.Email;
using Api.User.DTOs.Delet;
using System.IdentityModel.Tokens.Jwt;
using Api.Psychologist.Data.InterfaceSql;
using Api.Admin.Data.ServiceSql;
using Api.Admin.Data.InterfaceSql;



namespace Api.User.Services;

public class UserService : IUserInterface

{
    private readonly IUserSql _userSql;
    private readonly IAdminInterfaceSql _adminSql;
    private readonly IPsychologistInterfaceSql _psychologistSql;
    private readonly IAuthInterface _auth;
    public UserService(IUserSql user, IAuthInterface auth, IPsychologistInterfaceSql psychologist, IAdminInterfaceSql admin)
    {
        _userSql = user;
        _auth = auth;
        _psychologistSql = psychologist;
        _adminSql = admin;
    }
    public static bool TestCpf(string cpf){
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
                    return rest == 10 ? 0 : rest;}

        return CalcDigit(9, 10) == d[9] && CalcDigit(10, 11) == d[10];
    }

    public async Task<Result<ReturnUserDTO>> LoginAsync(LoginUserDTO dto)
    {
        var user = new UserModel(0, "", "", dto.CPF, "0", dto.Password, "");
        var data = await _userSql.LoginAsync(user);
        if (data.Success){
            int result = await _userSql.GetId(dto.CPF, data.Role);
            string token = _auth.NewToken(result, data.Role);
            
            var returnDTO = new ReturnUserDTO(result, token, data.Role);
            return new Result<ReturnUserDTO>()
            {
                Data = returnDTO,
                Success = true
            };
        }
        else{
            return new Result<ReturnUserDTO>()
            {
                Data = null,
                Success = false
            };
        }
    }

    public async Task<bool> EditAddressAsync(AddressEntryDTO dto)
    {
        var adress = new AddressModel(dto);
        var data = await _userSql.EditAddressAsync(adress);
        if (data)
        {
            return true;
        }
        return false;
    }

    public async Task<Result<bool>> CreateAddressAsync(AddressEntryDTO dto)
    {
        var adress = new AddressModel(dto);
        bool data = await _userSql.CreateAddressAsync(adress);
        if (!data)
        {
            return new Result<bool>
            {
                Success = false,
                Data = false
            };
        }
        return new Result<bool>
        {
            Success = true,
            Data = false
        };
    }

    public async Task<Result<bool>> CreatePhoneNumberAsync(PhoneNumberEntryDTO dto)
    {
        var number = new NumberModel(dto.Id, dto.Number, dto.CountryCode, dto.DDD, dto.IsEmergencyContact);
        bool data = await _userSql.CreatePhoneNumberAsync(number);
        if (!data)
        {
            return new Result<bool>
            {
                Success = false,
                Data = false
            };
        }
        return new Result<bool>
        {
            Success = true,
            Data = false
        };
    }

    public async Task<Result<bool>> CreateEmailAsync(EmailEntryDTO dto)
    {
        var email = new EmailModel(dto.Id, dto.Address, dto.Extension);
        bool data = await _userSql.CreateEmailAsync(email);
        if (!data)
        {
            return new Result<bool>
            {
                Success = false,
                Data = false
            };
        }
        return new Result<bool>
        {
            Success = true,
            Data = false
        };
    }

    public async Task<Result<bool>> DeletUserAsync(DeletUserDTO dto)
    {   
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(dto.Token);

        var id = jwt.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        var role = jwt.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

        if(role.ToUpper() == "A")
        {
            var data = await _adminSql.GetAdminByIdAsync(int.Parse(id));
            int AdminId = data.Id;
            string cpg = data.CPF;
        }
        else
        {
            var data = await _psychologistSql.GetPsychologistById(int.Parse(id));
            int PsychologistId = data.Id;
            string cpf = data.CPF;
        }


        return new Result<bool>
        {
            Success = true,
            Data = false,
            Message = "User delet with sucess"
        };
    }

    public async Task<Result<bool>> VerifyCPFAsync (string cpf){
        
        bool success = TestCpf(cpf);
        string message;

        if(success){
            message = "Cpf válido";
        }
        else{
            message = "Cpf inválido";
        }

        return new Result<bool>
        {
            Success = success,
            Data = false,
            Message = message
        };
        
    }

}