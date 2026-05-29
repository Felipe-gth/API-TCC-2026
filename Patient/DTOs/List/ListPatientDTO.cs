// namespace Api.Patient.DTOs.List;
// using System.ComponentModel.DataAnnotations;

// public class ListPatientDTO
// {
//     [Required(ErrorMessage = "Name ir required")]
//     public string Name { get; set; }

//     [Required(ErrorMessage = "LestName ir required")]
//     public string LastName { get; set; }

//     [Required (ErrorMessage = "CPF ir required")]
//     public string CPF { get; set; }

//     [Required (ErrorMessage = "Extension ir required")]
//     public string Extension { get; set; }

//     [Required (ErrorMessage = "Contrycode ir required")]
//     public string CountryCode { get; set; }
    
//     [Required (ErrorMessage = "DDD ir required")]
//     public string DDD { get; set;}
    

//     public ListPatientDTO(String name, string lastName, string cpf, string extension, string contrycode, string ddd ){
//         Name = name;
//         LastName = lastName;
//         CPF = cpf;
//         Extension = extension;
//         CountryCode = contrycode; 
//         DDD = ddd; 
//     }
// }

namespace Api.Patient.DTOs.List;

public class ListPatientDTO
{
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string Age { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    public ListPatientDTO() {}
}