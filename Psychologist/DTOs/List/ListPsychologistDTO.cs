namespace Api.Psychologist.DTOs.List;
using System.ComponentModel.DataAnnotations;

public class ListPsychologistDTO
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "LastName is required")]
    public string LastName { get; set; }

    [Required (ErrorMessage = "CPF is required")]
    public string CPF { get; set; }
    
    [Required (ErrorMessage = "CRP is required")]
    public string CRP { get; set;}
    public string Specialization  { get; set; }

    

    public ListPsychologistDTO(int id, string name, string lastName, string cpf, string crp, string specialization ){
        Id = id;
        Name = name;
        LastName = lastName;
        CPF = cpf;
        CRP = crp;
        Specialization = specialization;
    }
}