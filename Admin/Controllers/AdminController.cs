using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Api.Admin.DTOs.Register;
using Api.Admin.Models;
using Api.Admin.Interfaces;
namespace Api.Admin.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IAdmInterface _admin;
    public AdminController(IAdmInterface admin)
    {
        _admin = admin;
    }
    //[Authorize(Roles = "A")]
    [HttpPost("createAdmin")]
    public async Task<IActionResult> CreateAdmin([FromBody] RegisterAdminDTO dto)
    {
        try
        {
            var result = await _admin.CreateAdmin(dto);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest("Failed to create admin.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}