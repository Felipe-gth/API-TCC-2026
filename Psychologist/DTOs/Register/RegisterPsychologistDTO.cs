namespace Api.Psychologist.DTOs.Register;
using System.ComponentModel.DataAnnotations;
using Api.User.DTOs;

public class RegisterPsychologistDTO : UserModelDTO{

    [Required(ErrorMessage = "CRP is required")]
    [MinLength(7, ErrorMessage = "CRP requires 7 characters")]
    [MaxLength(7, ErrorMessage = "CRP requires 7 characters")]
    public string CRP {get; set;}

    [Required(ErrorMessage = "Specialization is required")]
    [MaxLength(50, ErrorMessage = "Specialization must be at most 50 characters long")]
    public string Specialization {get; set;}

}