namespace Api.Psychologist.DTOs.Register;
using System.ComponentModel.DataAnnotations;
using Api.User.DTOs;

public class EntryPsicologoDTO : UserModelDTO{

    [Required(ErrorMessage = "CRP is requiered")]
    [MinLength(7, ErrorMessage = "CRP requiered 7 character")]
    [MaxLength(7, ErrorMessage = "CRP requiered 7 character")]
    public string CRP {get; set;}

    [Required(ErrorMessage = "Especiacilization is requiered")]
    [MaxLength(50, ErrorMessage = "Especiacilization must be at most 50 characters long")]
    public string Espciacilization {get; set;}

}