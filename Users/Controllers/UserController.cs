<<<<<<< HEAD
﻿using Api.User.DTOs.Adress;
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
    private readonly IUserInterface _person;
    public UserController(IUserInterface person)
    {
        _person = person;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginUserDTO dto){
        try
        {
            var result = await _person.LoginAsync(dto);
            if (!result.Sucess)
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

    [HttpPut("editAdress")]
    public async Task<IActionResult> EditAdress([FromBody] AdressEntryDTO dto)
    {
        try
        {
            var result = await _person.EditAdressAsync(dto);
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
    [HttpPost("createAdress")]
    public async Task<IActionResult> CreateAdress([FromBody] AdressEntryDTO dto)
    {
        try
        {
            var result = await _person.CreateAdressAsync(dto);
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
            var result = await _person.CreatePhoneNumberAsync(dto);
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
            var result = await _person.CreateEmailAsync(dto);
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
    

=======
﻿using Api.User.DTOs.Adress;
using Api.User.DTOs.Login;
using Microsoft.AspNetCore.Mvc;
using Api.User.Interfaces;

namespace Api.User.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserInterface _person;
    public UserController(IUserInterface person)
    {
        _person = person;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginUserDTO dto){
        try
        {
            var result = await _person.LoginAsync(dto);
            if (!result.Sucess)
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

    [HttpPut("editAdress")]
    public async Task<IActionResult> EditAdress([FromBody] AdressEntryDTO dto)
    {
        try
        {
            var result = await _person.EditAdressAsync(dto);
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

    

>>>>>>> b3e63b1 (Pull rebase)
}