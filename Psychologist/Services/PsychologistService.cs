using Api.Psychologist.DTOs.Update;
using Api.Psychologist.Data.InterfaceSql;
using Api.Psychologist.DTOs.List;
using Api.Psychologist.DTOs.Register;
using Api.Shared.DTOs.Result;
using Api.Psychologist.Models;
using Api.User.DTOs.Return;
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
        if (result != null && result.Any())
        {
            var a = new Result<IEnumerable<ListPsychologistDTO>>
            {
                Success = true,
                Data = result
            };
            return a;
        }
        else
        {
            var a = new Result<IEnumerable<ListPsychologistDTO>>
            {
                Success = false,
                Data = null
            };
            return a;
        }
        
    }

    public async Task<Result<ReturnUserDTO>> RegisterPsychologist(RegisterPsychologistDTO dto)
    {
        var psychologist = new PsychologistModel(0, dto.Name, dto.LastName, dto.CPF, dto.Age, dto.Password, dto.Specialization, dto.CRP);
        int result = await _psychologistSQL.RegisterPsychologist(psychologist);
        if (result != 0)
        {
            var a = new Result<ReturnUserDTO>
            {
                Success = true,
                Data = new ReturnUserDTO(result, "", "P")
            };
            return a;
        }
        else
        {
            var a = new Result<ReturnUserDTO>
            {
                Success = false,
                Data = null
            };
            return a;
        }
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
            Success = true,
            Data = true
        };
    }
}