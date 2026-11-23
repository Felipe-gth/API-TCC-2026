namespace Api.Psychologist.DTOs.List;
using System.ComponentModel.DataAnnotations;

public class ListPsychologistDTO
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "LastName is required")]
    public string LastName { get; set; } = string.Empty;

    [Required (ErrorMessage = "CPF is required")]
    public string CPF { get; set; } = string.Empty;

    public string Age { get; set; } = string.Empty;

    [Required (ErrorMessage = "CRP is required")]
    public string CRP { get; set;} = string.Empty;
    public string Specialization  { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    
    // Necessário para o Dapper materializar o DTO quando o SELECT inclui isActive.
    public ListPsychologistDTO() { }

    public ListPsychologistDTO(int id, string name, string lastName, string cpf, string age, string crp, string specialization ){
        Id = id;
        Name = name;
        LastName = lastName;
        CPF = cpf;
        Age = age;
        CRP = crp;
        Specialization = specialization;
    }
}