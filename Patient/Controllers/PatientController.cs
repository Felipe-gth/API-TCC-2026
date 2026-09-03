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
    [Authorize(Roles = "P,A")]
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
            return StatusCode(500, $"Internal server error: {ex.Message}");
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
            return StatusCode(500, $"Internal server error: {ex.Message}");
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
            return StatusCode(500, $"Internal server error: {ex.Message}");
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
            return StatusCode(500, $"Internal server error: {ex.Message}");
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
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    
}