namespace Api.Psychologist.DTOs.Register;
using System.ComponentModel.DataAnnotations;
using Api.User.DTOs;

public class RegisterPsychologistDTO : UserModelDTO{

    [Required(ErrorMessage = "CRP is required")]
    [MinLength(8, ErrorMessage = "CRP requires 8 characters")]
    [MaxLength(8, ErrorMessage = "CRP requires 8 characters")]
    public string CRP {get; set;}
    
    [Required(ErrorMessage = "Specialization is required")]
    [MaxLength(50, ErrorMessage = "Specialization must be at most 50 characters long")]
    public string Specialization {get; set;}

}