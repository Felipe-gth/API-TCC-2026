using Api.Patient.DTOs.Edit;
using Api.Patient.DTOs.Register;
using Api.Patient.Interfaces;
using Api.User.DTOs.Adress;
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
    //[Authorize(Roles = "P")]
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
    
    [Authorize(Roles = "P")]
    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetPatientById(int id)
    {
        try
        {
            var result = await _patient.GetPatientByIdAsync(id);
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
    //[Authorize(Roles = "P,A")]
    [HttpGet("ListPatient")]
    public async Task<IActionResult> ListPatient()
    {
        try
        {
            var result = await _patient.ListPatient();
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