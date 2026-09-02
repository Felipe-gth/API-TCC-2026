using Api.User.DTOs;

namespace Api.Psychologist.DTOs.Update;
using System.ComponentModel.DataAnnotations;

public class UpdatePsychologistDTO : UserModelDTO
{
    [Required(ErrorMessage = "Id is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive integer")]
    public int Id { get; set; }

    [Required(ErrorMessage = "CRP is required")]
    [MinLength(8, ErrorMessage = "CRP required 8 characters")]
    [MaxLength(8, ErrorMessage = "CRP required 8 characters")]
    public string CRP { get; set; }

    [Required(ErrorMessage = "Specialization is required")]
    [MaxLength(50, ErrorMessage = "Specialization must be at most 50 characters long")]
    public string Specialization { get; set; }
}