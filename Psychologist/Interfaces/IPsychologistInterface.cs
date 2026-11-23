using Api.Psychologist.DTOs.Update;
using Api.Psychologist.DTOs.List;
using Api.Psychologist.DTOs.Register;
using Api.Shared.DTOs.Result;

namespace Api.Psychologist.Interfaces;

public interface IPsychologistInterface
{
    Task<Result<IEnumerable<ListPsychologistDTO>>> ListPsychologist();
    Task<Result<ListPsychologistDTO>> RegisterPsychologist(RegisterPsychologistDTO dto);
    Task<Result<bool>> EditPsychologist(UpdatePsychologistDTO dto);
    Task<Result<ListPsychologistDTO>> GetPsychologistById(int id);
    Task<Result<bool>> DeletePsychologist(int id);
}