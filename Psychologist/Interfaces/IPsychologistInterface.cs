using Api.Psychologist.DTOs.Update;
using Api.Psychologist.DTOs.List;
using Api.Psychologist.DTOs.Register;
using Api.Shared.DTOs.Result;
using Api.User.DTOs.Return;

namespace Api.Psychologist.Interfaces;

public interface IPsychologistInterface
{
    Task<Result<IEnumerable<ListPsychologistDTO>>> ListPsychologist();
    Task<Result<ReturnUserDTO>> RegisterPsychologist(RegisterPsychologistDTO dto);
    Task<Result<bool>> EditPsychologist(UpdatePsychologistDTO dto);
}