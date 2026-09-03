using System.Runtime.InteropServices.JavaScript;
using Api.Patient.Data.InterfaceSql;
using Api.Patient.DTOs.Edit;
using Api.Patient.DTOs.List;
using Api.Patient.DTOs.Register;
using Api.Shared.DTOs.Result;
using Api.Patient.Interfaces;
using Api.User.DTOs.Address;
using Api.User.DTOs.Email;
using Api.User.DTOs.Phone;
using Api.User.DTOs.Return;
using Api.User.DTOs.Return.Address;
using Api.User.DTOs.Return.Email;
using Api.User.DTOs.Return.Phone;
using Api.User.Models;

namespace Api.Patient.Services;

public class PatientService : IPatientInterface
{
    private readonly IPatientInterfaceSql _patientSQL;
    public PatientService(IPatientInterfaceSql patient)
    {
        _patientSQL = patient;
    }

    //Register

    public async Task<Result<ListPatientDTO>> CreatePatientAsync(RegisterPatientDTO dto)
    {
        var user = new UserModel(0, dto.Name, dto.LastName, dto.CPF, dto.Age, dto.Password, "C");
        int data = await _patientSQL.CreatePatientAsync(user);
        if (data <= 0)
        {
            var result = new Result<ListPatientDTO>
            {
                Success = false,
                Data = null

            };
            return result;
        }
        var created = await _patientSQL.GetPatientFromIdAsync(data);
        if (created == null)
        {
            created = new ListPatientDTO
            {
                Id = data,
                Name = dto.Name,
                LastName = dto.LastName,
                CPF = dto.CPF,
                Age = dto.Age,
                Role = "C"
            };
        }
        return new Result<ListPatientDTO>
        {
            Success = true,
            Data = created
        };
    }

    //Login


    

    public async Task<Result<ListPatientDTO>> GetPatientByIdAsync(int id)
    {
        var patient = await _patientSQL.GetPatientFromIdAsync(id);
        if (patient != null)
        {
            return new Result<ListPatientDTO>
            {
                Success = true,
                Data = patient
            };
        }
        return new Result<ListPatientDTO>
        {
            Success = false,
            Data = null
        };
    }

    public async Task<Result<IEnumerable<ListPatientDTO>>> ListPatient(int? psychologistId = null)
    {
        var result = psychologistId.HasValue
            ? await _patientSQL.ListPatientsByPsychologist(psychologistId.Value)
            : await _patientSQL.ListAllPatient();

        if (result != null)
        {
            return new Result<IEnumerable<ListPatientDTO>>
            {
                Success = true,
                Data = result
            };
        }
        return new Result<IEnumerable<ListPatientDTO>>
        {
            Success = false,
            Data = null
        };
    }
    
    public async Task<Result<bool>> EditPatientAsync(EditPatientDTO dto)
    {
        var patient = new UserModel(dto.Id, dto.Name, dto.LastName, dto.CPF, dto.Age, dto.Password, "C");
        bool data = await _patientSQL.EditPatientAsync(patient);
        if (data)
        {
            return new Result<bool>
            {
                Success = true,
                Data = true
            };
        }
        return new Result<bool>
        {
            Success = false,
            Data = false
        };
    }

    public async Task<Result<bool>> LinkPatientToPsychologist(LinkPatientPsychologistDTO dto)
    {
        var ok = await _patientSQL.LinkPatientToPsychologistSql(dto.PatientId, dto.PsychologistId);
        return new Result<bool>(ok, ok);
    }

    public async Task<Result<ReturnPatientPsychologistDTO>> GetPatientPsychologist(int patientId)
    {
        var psychologist = await _patientSQL.GetActivePsychologistSql(patientId);
        if (psychologist != null)
        {
            return new Result<ReturnPatientPsychologistDTO>(true, psychologist);
        }
        return new Result<ReturnPatientPsychologistDTO>(false, null);
    }
}