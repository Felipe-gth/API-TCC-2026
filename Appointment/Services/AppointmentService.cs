namespace Api.Appointment.Services;


using Api.Appointment.Interface;
using Api.Appointment.Data.Interfaces;
using Api.Appointment.Models;
using Api.Shared.DTOs.Result;
using Api.Appointment.DTOs.Create;
using Api.Appointment.DTOs.Return;

public class AppointmentService : IAppointmentInterface
{
    private readonly IAppointmentInterfaceSql _appointmentSql;
    public AppointmentService(IAppointmentInterfaceSql appointmentSql)
    {
        _appointmentSql = appointmentSql;
    }

    public async Task<Result<ReturnAppointmentDTOsimple>> CreateAppointment (CreateAppointmentDTO dto)
    {
        string Type;
        var model = new AppointmentModel(dto.Type, dto.DateAndTime, dto.Notes, dto.MaritalStatus, dto.PhysicalHealth, dto.HadTreatment, dto.Habits, dto.SearchReason, dto.PatientId, dto.PsychologistId);
        var (success, id) = await _appointmentSql.CreateAppointmentSql(model);
        //TODO: split model.DateAndTime to get: Month, Day and Hour.
        string[]? papoi;
        string dateAndTime = model.Date.ToString();
        papoi = dateAndTime.Split(new char[] {'/', ' ', '-', ':' }, StringSplitOptions.RemoveEmptyEntries);
        if (model.Type == "O"){
            Type = "Online";
        }
        else if(model.Type == "P")
        {
            Type = "Presencial";
        }
        else {
            Type = "P";
        }
        
        if (success)
        {
            var appointmentReturn = new ReturnAppointmentDTOsimple(id, Type, int.Parse(papoi[1]), int.Parse(papoi[0]), int.Parse(papoi[3]), model.PatientId, model.PsychologistId);
            var resultReturn = new Result<ReturnAppointmentDTOsimple>(true, appointmentReturn);  
            return resultReturn;
        }
        var resultReturnError = new Result<ReturnAppointmentDTOsimple>(false, null);
        return resultReturnError;
    }
}
