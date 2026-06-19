using System.ComponentModel.DataAnnotations;

namespace Api.Patient.DTOs.Edit;

public class EditPatientDTO
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string CPF { get; set; }
    [Required]
    public string Age { get; set; }
    [Required]
    public string Password { get; set; }


    public EditPatientDTO(int id, string name, string lastName, string cpf, string age, string password)
    {
        Id = id;
        Name = name;
        LastName = lastName;
        CPF = cpf;
        Age = age;
        Password = password;
    }
}