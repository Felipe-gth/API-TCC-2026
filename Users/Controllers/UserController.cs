using Api.User.DTOs.Address;
using Api.User.DTOs.Login;
using Microsoft.AspNetCore.Mvc;
using Api.User.Interfaces;
using Api.User.DTOs.Email;
using Microsoft.AspNetCore.Authorization;
using Api.User.DTOs.Delet;
using Api.User.DTOs.Phone;
using Api.User.DTOs.Return.Address;
using Api.User.DTOs.Return.Phone;
using Api.User.DTOs.Return.Email;

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
    public async Task<IActionResult> LoginAsync([FromBody] LoginUserDTO dto)
    {
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
            if (result.Success)
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
            if (result.Success)
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
            if (result.Success)
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
            if (result.Success)
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
    [HttpGet("addresses/{patientId}")]
    public async Task<IActionResult> GetAddresses(int patientId)
    {
        try
        {
            var result = await _userService.GetAddressesByPatientIdAsync(patientId);
            if (result.Success)
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
    [HttpGet("phones/{patientId}")]
    public async Task<IActionResult> GetPhoneNumbers(int patientId)
    {
        try
        {
            var result = await _userService.GetPhoneNumbersByPatientIdAsync(patientId);
            if (result.Success)
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
    [HttpGet("emails/{patientId}")]
    public async Task<IActionResult> GetEmails(int patientId)
    {
        try
        {
            var result = await _userService.GetEmailsByPatientIdAsync(patientId);
            if (result.Success)
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

    // Update endpoints
    [Authorize(Roles = "C")]
    [HttpPut("addresses/{addressId}")]
    public async Task<IActionResult> UpdateAddress(int addressId, [FromBody] AddressEntryDTO dto)
    {
        try
        {
            var result = await _userService.UpdateAddressAsync(addressId, dto);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [Authorize(Roles = "C")]
    [HttpPut("phones/{phoneId}")]
    public async Task<IActionResult> UpdatePhoneNumber(int phoneId, [FromBody] PhoneNumberEntryDTO dto)
    {
        try
        {
            var result = await _userService.UpdatePhoneNumberAsync(phoneId, dto);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [Authorize(Roles = "C")]
    [HttpPut("emails/{emailId}")]
    public async Task<IActionResult> UpdateEmail(int emailId, [FromBody] EmailEntryDTO dto)
    {
        try
        {
            var result = await _userService.UpdateEmailAsync(emailId, dto);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // Delete endpoints
    [Authorize(Roles = "C")]
    [HttpDelete("addresses/{addressId}")]
    public async Task<IActionResult> DeleteAddress(int addressId)
    {
        try
        {
            var result = await _userService.DeleteAddressAsync(addressId);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [Authorize(Roles = "C")]
    [HttpDelete("phones/{phoneId}")]
    public async Task<IActionResult> DeletePhoneNumber(int phoneId)
    {
        try
        {
            var result = await _userService.DeletePhoneNumberAsync(phoneId);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [Authorize(Roles = "C")]
    [HttpDelete("emails/{emailId}")]
    public async Task<IActionResult> DeleteEmail(int emailId)
    {
        try
        {
            var result = await _userService.DeleteEmailAsync(emailId);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [Authorize]
    [HttpDelete("delete")]
    public async Task<IActionResult> DeletUserAsync([FromBody] DeletUserDTO dto)
    {
        try
        {
            var result = await _userService.DeletUserAsync(dto);
            if (result.Success)
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