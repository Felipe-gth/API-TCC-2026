using Api.User.DTOs.Address;
using Api.User.DTOs.Login;
using Microsoft.AspNetCore.Mvc;
using Api.User.Interfaces;
using Api.User.DTOs.Email;
using Microsoft.AspNetCore.Authorization;
using Api.User.DTOs.Phone;

namespace Api.User.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserInterface _userService;
    public UserController(IUserInterface userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginUserDTO dto){
        try
        {
            var result = await _userService.LoginAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("editAddress")]
    public async Task<IActionResult> EditAddress([FromBody] AddressEntryDTO dto)
    {
        try
        {
            var result = await _userService.EditAddressAsync(dto);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    [Authorize(Roles = "C")]
    [HttpPost("createAddress")]
    public async Task<IActionResult> CreateAddress([FromBody] AddressEntryDTO dto)
    {
        try
        {
            var result = await _userService.CreateAddressAsync(dto);
            if (result.Data != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [Authorize(Roles = "C")]
    [HttpPost("createNumber")]
    public async Task<IActionResult> CreatePhoneNumberAsync([FromBody] PhoneNumberEntryDTO dto)
    {
        try
        {
            var result = await _userService.CreatePhoneNumberAsync(dto);
            if (result.Data != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    [Authorize(Roles = "C")]
    [HttpPost("createEmail")]
    public async Task<IActionResult> CreateEmailAsync([FromBody] EmailEntryDTO dto)
    {
        try
        {
            var result = await _userService.CreateEmailAsync(dto);
            if (result.Data != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    

}