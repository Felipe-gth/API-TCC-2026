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

    public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentDTO dto)
    {
        var result = await _appointment.CreateAppointment(dto);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest();
        
    }
}