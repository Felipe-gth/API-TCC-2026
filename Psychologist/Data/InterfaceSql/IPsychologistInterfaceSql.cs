namespace Api.Psychologist.Data.InterfaceSql;

using Api.Psychologist.DTOs.List;
using Api.Psychologist.Models;

public interface IPsychologistInterfaceSql
{
    Task<IEnumerable<ListPsychologistDTO>> ListPsychologist();
    Task<int> RegisterPsychologist(PsychologistModel p);
    Task<int> EditPsychologist(PsychologistModel p);
    Task<ListPsychologistDTO> GetPsychologistById(int id);
    Task<bool> DeletePsychologist(int id);
}
