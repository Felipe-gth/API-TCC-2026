namespace Api.Psychologist.DTOs.List;
using System.ComponentModel.DataAnnotations;

public class ListPsicologoDTO
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Name ir required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "LestName ir required")]
    public string LastName { get; set; }

    [Required (ErrorMessage = "CPF ir required")]
    public string CPF { get; set; }
    
    [Required (ErrorMessage = "CRM ir required")]
    public string CRP { get; set;}
    public string Specialization  { get; set; }

    

    public ListPsicologoDTO(int id, string name, string lastName, string cpf, string crp, string specialization ){
        Id = id;
        Name = name;
        LastName = lastName;
        CPF = cpf;
        CRP = crp;
        Specialization = specialization;
    }
}