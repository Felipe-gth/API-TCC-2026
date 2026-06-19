using API.DTOs.Psychologist.Update;
using Api.Psychologist.Data.InterfaceSql;
using Api.Psychologist.DTOs.List;
using Api.Psychologist.DTOs.Register;
using Api.Psychologist.DTOs.Result;
using Api.Psychologist.Models;
using Api.User.DTOs.Return;
using tcc.Psychologist.Interfaces;


namespace Api.Psychologist.Services;

public class PsychologistService : IPsychologistInterface
{
    private readonly IPsychologistInterfaceSql _psicologoSQL;
    public PsychologistService(IPsychologistInterfaceSql psicologoSQL)
    {
        _psicologoSQL = psicologoSQL;
    }
    public async Task<Result<IEnumerable<ListPsicologoDTO>>> ListPsicologo()
    {
        var result = await _psicologoSQL.ListPsicologo();
        if (result != null && result.Any())
        {
            var a = new Result<IEnumerable<ListPsicologoDTO>>
            {
                Sucess = true,
                Data = result
            };
            return a;
        }
        else
        {
            var a = new Result<IEnumerable<ListPsicologoDTO>>
            {
                Sucess = false,
                Data = null
            };
            return a;
        }
        
    }

    public async Task<Result<ReturnUserDTO>> RegisterPsicologo(EntryPsicologoDTO dto)
    {
        var psciologo = new PsychologistModel(0, dto.Name, dto.LastName, dto.CPF, dto.Age, dto.Password, dto.Espciacilization, dto.CRP);
        int result = await _psicologoSQL.RegisterPsicologo(psciologo);
        if (result != 0)
        {
            var a = new Result<ReturnUserDTO>
            {
                Sucess = true,
                Data = new ReturnUserDTO(result, "", "P")
            };
            return a;
        }
        else
        {
            var a = new Result<ReturnUserDTO>
            {
                Sucess = false,
                Data = null
            };
            return a;
        }
    }

    public async Task<Result<bool>> EditPsicologo(UpdatePsicologoDTO dto)
    {
        var psicologo = new PsychologistModel(dto.Id, dto.Name, dto.LastName, dto.CPF, dto.Age, dto.Password, dto.Espciacilization, dto.CRP);
        int rowsAffected = await _psicologoSQL.EditPsicologo(psicologo);
        if (rowsAffected > 0)
        {
            return new Result<bool>
            {
                Sucess = true,
                Data = true
            };
        }

        return new Result<bool>
        {
            Sucess = true,
            Data = true
        };
    }
}