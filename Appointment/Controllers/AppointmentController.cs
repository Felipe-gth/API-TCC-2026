using System.Security.Cryptography.X509Certificates;
using Api.Appointment.DTOs.Create;
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

    [HttpPost("Create")]
    public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentDTO dto)
    {
        var result = await _appointment.CreateAppointment(dto);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest();
        
    }

    //[Authorize(Roles = "P,A")]
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
}