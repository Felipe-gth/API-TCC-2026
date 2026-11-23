using Api.Psychologist.DTOs.Update;
using Api.Psychologist.DTOs.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Api.Psychologist.Interfaces;

namespace Api.Psychologist.Controllers;

//[Authorize(Roles = "A")]
[ApiController]
[Route("api/[controller]")]

public class PsychologistController : ControllerBase
{
    private readonly IPsychologistInterface _psychologist;
    public PsychologistController(IPsychologistInterface psychologist)
    {
        _psychologist = psychologist;
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListPsychologist()
    {
        try
        {
            var result = await _psychologist.ListPsychologist();
            if (result.Data != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[500] {ex}");
            return StatusCode(500, "Internal server error.");
        }
        
    }
    [HttpPost("register")]
    public async Task<IActionResult> RegisterPsychologist([FromBody] RegisterPsychologistDTO dto)
    {
        try
        {
            var result = await _psychologist.RegisterPsychologist(dto);
            if (result.Data != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[500] {ex}");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPut("edit")]
    public async Task<IActionResult> EditPsychologist([FromBody] UpdatePsychologistDTO dto)
    {
        try
        {
            var result = await _psychologist.EditPsychologist(dto);
            if (result.Data)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[500] {ex}");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet("get-by-id/{id}")]
    public async Task<IActionResult> GetPsychologistById(int id)
    {
        try
        {
            var result = await _psychologist.GetPsychologistById(id);
            if (result.Data != null)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[500] {ex}");
            return StatusCode(500, "Internal server error.");
        }
    }

    [Authorize(Roles = "A")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePsychologist(int id)
    {
        try
        {
            var result = await _psychologist.DeletePsychologist(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[500] {ex}");
            return StatusCode(500, "Internal server error.");
        }
    }
}