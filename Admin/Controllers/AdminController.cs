using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Api.Admin.DTOs.Register;
using Api.Admin.Models;
using Api.Admin.Interfaces;
using Api.Shared.DTOs.Result;
namespace Api.Admin.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IAdminInterface _admin;
    public AdminController(IAdminInterface admin)
    {
        _admin = admin;
    }
    //[Authorize(Roles = "A")]
    [HttpPost("createAdmin")]
    public async Task<IActionResult> CreateAdminAsync([FromBody] RegisterAdminDTO dto)
    {
        try
        {
            var result = await _admin.CreateAdminAsync(dto);
            if (result.Success)
            {
                return Ok(result);
            }
            if (result.Message == "CPF ja cadastrado.")
            {
                return Conflict(result);
            }
            return BadRequest(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[500] {ex}");
            return StatusCode(500, "Internal server error.");
        }
    }
}