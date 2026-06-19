using API.DTOs.Psychologist.Update;
using Api.Psychologist.DTOs.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tcc.Psychologist.Interfaces;

namespace Api.Psychologist.Controllers;

//[Authorize(Roles = "A")]
[ApiController]
[Route("api/[controller]")]

public class PsychologistController : ControllerBase
{
    private readonly IPsychologistInterface _psicologo;
    public PsychologistController(IPsychologistInterface psicologo)
    {
        _psicologo = psicologo;
    }

    [Authorize(Roles = "A")]
    [HttpGet("/api/ListPsicologo")]
    public async Task<IActionResult> ListPsicologo()
    {
        try
        {
            var result = await _psicologo.ListPsicologo();
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
            return StatusCode(500, "Internal Error: " + ex.Message);
        }
        
    }
    [HttpPost("/api/RegisterPsicologo")]
    public async Task<IActionResult> RegisterPsicologo([FromBody] EntryPsicologoDTO dto)
    {
        try
        {
            var result = await _psicologo.RegisterPsicologo(dto);
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
            return StatusCode(500, "Internal Error: " + ex.Message);
        }
    }

    [HttpPut("/api/EditPsicologo")]
    public async Task<IActionResult> EditPsicologo([FromBody] UpdatePsicologoDTO dto)
    {
        try
        {
            var result = await _psicologo.EditPsicologo(dto);
            if (result.Data)
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
            return StatusCode(500, "Internal Error: " + ex.Message);
        }
    }
}