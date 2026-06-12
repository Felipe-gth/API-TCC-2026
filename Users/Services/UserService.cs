
using Api.User.DTOs.Adress;
using Data.Interface;
using Api.User.DTOs.Return;
using Api.User.DTOs.Result;
using Api.User.Interfaces;
using Api.User.DTOs.Login;
using Api.User.Models;
using Api.User.DTOs.Phone;
using Api.User.DTOs.Email;


namespace Api.User.Services;

public class UserService : IUserInterface

{
    private readonly IUserSQL _userSql;
    private readonly IAuthInterface _auth;
    public UserService(IUserSQL user, IAuthInterface auth)
    {
        _userSql = user;
        _auth = auth;
    }


    public async Task<Result<ReturnUserDTO>> LoginAsync(LoginUserDTO dto)
    {
        var user = new UserModel(0, "", "", dto.CPF, "0", dto.Password, "");
        var data = await _userSql.LoginAsync(user);
        if (data.Success){
            int result = await _userSql.GetId(dto.CPF, data.Role);
            string token = _auth.newToken(result, data.Role);
            
            var returnDTO = new ReturnUserDTO(result, token, data.Role);
            return new Result<ReturnUserDTO>()
            {
                Data = returnDTO,
                Sucess = true
            };
        }
        else{
            return new Result<ReturnUserDTO>()
            {
                Data = null,
                Sucess = false
            };
        }
    }

    public async Task<bool> EditAdressAsync(AdressEntryDTO dto)
    {
        var adress = new AdressModel(dto);
        var data = await _userSql.EditAdressAsync(adress);
        if (data)
        {
            return true;
        }
        return false;
    }

    public async Task<Result<bool>> CreateAdressAsync(AdressEntryDTO dto)
    {
        var adress = new AdressModel(dto);
        bool data = await _userSql.CreateAdressAsync(adress);
        if (!data)
        {
            return new Result<bool>
            {
                Sucess = false,
                Data = false
            };
        }
        return new Result<bool>
        {
            Sucess = true,
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
                Sucess = false,
                Data = false
            };
        }
        return new Result<bool>
        {
            Sucess = true,
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
                Sucess = false,
                Data = false
            };
        }
        return new Result<bool>
        {
            Sucess = true,
            Data = false
        };
    }

}