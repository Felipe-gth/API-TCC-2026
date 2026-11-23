using Api.Patient.DTOs.Edit;
using Api.Patient.DTOs.Register;
using Api.Patient.Interfaces;
using Api.User.DTOs.Address;
using Api.User.DTOs.Email;
using Api.User.DTOs.Phone;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Patient.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientController : ControllerBase
{
    private readonly IPatientInterface _patient;
    public PatientController(IPatientInterface patient)
    {
        _patient = patient;
    }
    //[Authorize(Roles = "P,A")]
    [HttpPost("createPatient")]
    public async Task<IActionResult> CreatePatient([FromBody] RegisterPatientDTO dto)
    {
        try
        {
            var result = await _patient.CreatePatientAsync(dto);
            if (result.Data != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[500] {ex}");
            return StatusCode(500, "Internal server error.");
        }
    }

    [Authorize(Roles = "C,P,A")]
    [HttpGet("get-by-id/{id}")]
    public async Task<IActionResult> GetPatientById(int id)
    {
        try
        {
            var result = await _patient.GetPatientByIdAsync(id);
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
    [Authorize(Roles = "P,A")]
    [HttpGet("list")]
    public async Task<IActionResult> ListPatient([FromQuery] int? psychologistId = null)
    {
        try
        {
            var result = await _patient.ListPatient(psychologistId);
            if (result.Data != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[500] {ex}");
            return StatusCode(500, "Internal server error.");
        }
    }

    [Authorize(Roles = "A")]
    [HttpGet("list-not-active")]
    public async Task<IActionResult> ListPatientNotActive()
    {
        try
        {
            var result = await _patient.ListPatientNotActive();
            return Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[500] {ex}");
            return StatusCode(500, "Internal server error.");
        }
    }

    [Authorize(Roles = "A,P")]
    [HttpPost("link-psychologist")]
    public async Task<IActionResult> LinkPatientToPsychologist([FromBody] LinkPatientPsychologistDTO dto)
    {
        try
        {
            var result = await _patient.LinkPatientToPsychologist(dto);
            if (result.Success && result.Data)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[500] {ex}");
            return StatusCode(500, "Internal server error.");
        }
    }

    [Authorize(Roles = "C,P,A")]
    [HttpGet("{patientId}/psychologist")]
    public async Task<IActionResult> GetPatientPsychologist(int patientId)
    {
        try
        {
            var result = await _patient.GetPatientPsychologist(patientId);
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

    // Edit patient endpoint
    [Authorize(Roles = "C,P,A")]
    [HttpPut("edit")]
    public async Task<IActionResult> EditPatient([FromBody] EditPatientDTO dto)
    {
        try
        {
            // Paciente só edita o próprio cadastro.
            if (User.IsInRole("C"))
            {
                var rawId = User.FindFirst("id")?.Value;
                if (!int.TryParse(rawId, out var tokenId) || tokenId != dto.Id)
                {
                    return Forbid();
                }
            }

            // Psicólogo só edita pacientes vinculados a ele; adm edita qualquer um.
            int? psychologistId = null;
            if (User.IsInRole("P"))
            {
                psychologistId = GetPsychologistIdFromToken();
                if (psychologistId == null)
                {
                    return Forbid();
                }
            }

            var result = await _patient.EditPatientAsync(dto, psychologistId);
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

    [Authorize(Roles = "P,A")]
[HttpGet("{psychologistId}/listPatientNotActive")]
public async Task<IActionResult> ListPatientNotActiveFromPsychologist(
    int psychologistId)
{
    var result = await _patient.ListPatientNotActiveFromPsychologist(psychologistId);

    if (result.Data != null)
        return Ok(result);

    return NotFound();
}

    // Admin mexe com qualquer paciente; psicólogo só com os vinculados a ele.
    private int? GetPsychologistIdFromToken()
    {
        if (User.IsInRole("A"))
        {
            return null;
        }

        var rawId = User.FindFirst("id")?.Value;
        return int.TryParse(rawId, out var parsed) ? parsed : null;
    }

    [Authorize(Roles = "P,A")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePatient(int id)
    {
        try
        {
            var psychologistId = GetPsychologistIdFromToken();
            if (psychologistId == null && !User.IsInRole("A"))
            {
                return Forbid();
            }

            var result = await _patient.DeletePatient(id, psychologistId);
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

    [Authorize(Roles = "P,A")]
    [HttpPut("reactivate/{id}")]
    public async Task<IActionResult> ReactivatePatient(int id)
    {
        try
        {
            var psychologistId = GetPsychologistIdFromToken();
            if (psychologistId == null && !User.IsInRole("A"))
            {
                return Forbid();
            }

            var result = await _patient.ReactivatePatient(id, psychologistId);
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