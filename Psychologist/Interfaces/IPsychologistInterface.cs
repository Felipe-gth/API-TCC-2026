using API.DTOs.Psychologist.Update;
using Api.Psychologist.DTOs.List;
using Api.Psychologist.DTOs.Register;
using Api.Psychologist.DTOs.Result;
using Api.User.DTOs.Return;

namespace tcc.Psychologist.Interfaces;

public interface IPsychologistInterface
{
    Task<Result<IEnumerable<ListPsicologoDTO>>> ListPsicologo();
    Task<Result<ReturnUserDTO>> RegisterPsicologo(EntryPsicologoDTO dto);
    Task<Result<bool>> EditPsicologo(UpdatePsicologoDTO dto);
}