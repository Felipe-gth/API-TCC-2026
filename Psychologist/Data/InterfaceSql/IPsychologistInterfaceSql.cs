namespace Api.Psychologist.Data.InterfaceSql;

using Api.Psychologist.DTOs.List;
using Api.Psychologist.Models;
using Api.User.DTOs.Return;

public interface IPsychologistInterfaceSql
{
    Task<IEnumerable<ListPsicologoDTO>> ListPsicologo();
    Task<int> RegisterPsicologo(PsychologistModel p);
    Task<int> EditPsicologo(PsychologistModel p);
}