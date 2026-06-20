namespace Api.Psychologist.Data.InterfaceSql;

using Api.Psychologist.DTOs.List;
using Api.Psychologist.Models;
using Api.User.DTOs.Return;

public interface IPsychologistInterfaceSql
{
    Task<IEnumerable<ListPsychologistDTO>> ListPsychologist();
    Task<int> RegisterPsychologist(PsychologistModel p);
    Task<int> EditPsychologist(PsychologistModel p);
}