
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

}