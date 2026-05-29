using Api.User.DTOs;

namespace API.DTOs.Psychologist.Update;
using System.ComponentModel.DataAnnotations;

public class UpdatePsicologoDTO : UserModelDTO
{
    [Required(ErrorMessage = "Id is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive integer")]
    public int Id { get; set; }

    [Required(ErrorMessage = "CRP is required")]
    [MinLength(7, ErrorMessage = "CRP required 7 characters")]
    [MaxLength(7, ErrorMessage = "CRP required 7 characters")]
    public string CRP { get; set; }

    [Required(ErrorMessage = "Especiacilization is required")]
    [MaxLength(50, ErrorMessage = "Especiacilization must be at most 50 characters long")]
    public string Espciacilization { get; set; }
}