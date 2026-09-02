using Api.Psychologist.DTOs.Update;
using Api.Psychologist.Data.InterfaceSql;
using Api.Psychologist.DTOs.List;
using Api.Psychologist.DTOs.Register;
using Api.Shared.DTOs.Result;
using Api.Psychologist.Models;
using Api.Psychologist.Interfaces;


namespace Api.Psychologist.Services;

public class PsychologistService : IPsychologistInterface
{
    private readonly IPsychologistInterfaceSql _psychologistSQL;
    public PsychologistService(IPsychologistInterfaceSql psychologistSQL)
    {
        _psychologistSQL = psychologistSQL;
    }
    public async Task<Result<IEnumerable<ListPsychologistDTO>>> ListPsychologist()
    {
        var result = await _psychologistSQL.ListPsychologist();
        if (result != null)
        {
            return new Result<IEnumerable<ListPsychologistDTO>>
            {
                Success = true,
                Data = result
            };
        }
        return new Result<IEnumerable<ListPsychologistDTO>>
        {
            Success = false,
            Data = null
        };
    }

    public async Task<Result<ListPsychologistDTO>> RegisterPsychologist(RegisterPsychologistDTO dto)
    {
        var psychologist = new PsychologistModel(0, dto.Name, dto.LastName, dto.CPF, dto.Age, dto.Password, dto.Specialization, dto.CRP);
        int result = await _psychologistSQL.RegisterPsychologist(psychologist);
        if (result != 0)
        {
            return new Result<ListPsychologistDTO>
            {
                Success = true,
                Data = new ListPsychologistDTO(result, dto.Name, dto.LastName, dto.CPF, dto.CRP, dto.Specialization)
            };
        }
        return new Result<ListPsychologistDTO>
        {
            Success = false,
            Data = null
        };
    }

    public async Task<Result<bool>> EditPsychologist(UpdatePsychologistDTO dto)
    {
        var psychologist = new PsychologistModel(dto.Id, dto.Name, dto.LastName, dto.CPF, dto.Age, dto.Password, dto.Specialization, dto.CRP);
        int rowsAffected = await _psychologistSQL.EditPsychologist(psychologist);
        if (rowsAffected > 0)
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

    public async Task<Result<ListPsychologistDTO>> GetPsychologistById(int id)
    {
        var psychologist = await _psychologistSQL.GetPsychologistById(id);
        if (psychologist != null)
        {
            return new Result<ListPsychologistDTO>
            {
                Success = true,
                Data = psychologist
            };
        }
        return new Result<ListPsychologistDTO>
        {
            Success = false,
            Data = null
        };
    }

    public async Task<Result<bool>> DeletePsychologist(int id)
    {
        bool deleted = await _psychologistSQL.DeletePsychologist(id);
        return new Result<bool>(deleted, deleted);
    }
}