namespace Api.Psychologist.Data.InterfaceSql;

using Api.Psychologist.DTOs.List;
using Api.Psychologist.Models;

public interface IPsychologistInterfaceSql
{
    Task<IEnumerable<ListPsychologistDTO>> ListPsychologist();
    Task<IEnumerable<ListPsychologistDTO>> ListPsychologistNotActive();
    Task<int> RegisterPsychologist(PsychologistModel p);
    Task<int> EditPsychologist(PsychologistModel p);
    Task<ListPsychologistDTO> GetPsychologistById(int id);
    Task<int> DeletePsychologist(int id);
    Task<int> ReactivatePsychologist(int id);
}
