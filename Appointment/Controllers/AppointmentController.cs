using System.Security.Cryptography.X509Certificates;
using Api.Appointment.DTOs.Create;
using Api.Appointment.DTOs.Update;
using Api.Appointment.Interface;
using Api.Patient.DTOs.Edit;
using Api.Patient.DTOs.Register;
using Api.Patient.Interfaces;
using Api.User.DTOs.Address;
using Api.User.DTOs.Email;
using Api.User.DTOs.Phone;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Appointment.Controllers;

[ApiController]
[Route("api/[controller]")]

public class AppointmentController : ControllerBase
{
    private readonly IAppointmentInterface _appointment;
    public AppointmentController(IAppointmentInterface appointment)
    {
        _appointment = appointment;
    }

    [Authorize(Roles = "C,P,A")]
    [HttpPost("Create")]
    public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentDTO dto)
    {
        var result = await _appointment.CreateAppointment(dto);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [Authorize(Roles = "P,A")]
    [HttpGet("get-by-id/{id}")]
    public async Task<IActionResult> GetAppointmentById(int id)
    {
        try
        {
            var result = await _appointment.GetAppointmentById(id);
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

    [Authorize(Roles = "C,P,A")]
    [HttpGet("availability/{psychologistId}")]
    public async Task<IActionResult> GetAvailabilityByDate(int psychologistId, [FromQuery] DateOnly date)
    {
        try
        {
            var result = await _appointment.GetAvailabilityByDate(psychologistId, date);
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

    [Authorize(Roles = "P")]
    [HttpPost("availability")]
    public async Task<IActionResult> CreateAvailabilityDays([FromBody] CreateServiceDaysDTO dto)
    {
        try
        {
            var result = await _appointment.CreateAvailabilityDays(dto);
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
    [HttpGet("patient-agenda/{patientId}")]
    public async Task<IActionResult> GetPatientAgenda(int patientId)
    {
        try
        {
            var result = await _appointment.GetPatientAgenda(patientId);
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

    [Authorize(Roles = "C,P,A")]
    [HttpGet("patient/{patientId}")]
    public async Task<IActionResult> GetPatientAppointments(int patientId)
    {
        try
        {
            var result = await _appointment.GetPatientAppointments(patientId);
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
    [HttpGet("psychologist/{psychologistId}")]
    public async Task<IActionResult> GetPsychologistAppointments(int psychologistId)
    {
        try
        {
            var result = await _appointment.GetPsychologistAppointments(psychologistId);
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

    [Authorize(Roles = "C,P,A")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateAppointmentStatus([FromBody] EntryUpdateAppointmentStatusDTO dto)
    {
        try
        {
            var result = await _appointment.UpdateAppointmentStatus(dto);
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
}