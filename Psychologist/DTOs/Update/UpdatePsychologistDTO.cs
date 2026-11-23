namespace Api.Psychologist.DTOs.Update;
using System.ComponentModel.DataAnnotations;

public class UpdatePsychologistDTO
{
    [Required(ErrorMessage = "Id is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive integer")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "LastName is required")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "CPF is required")]
    [StringLength(11, MinimumLength = 11)]
    public string CPF { get; set; } = string.Empty;

    [Required(ErrorMessage = "Age is required")]
    [Range(1, 100)]
    public string Age { get; set; } = string.Empty;

    [Required(ErrorMessage = "CRP is required")]
    [MinLength(8, ErrorMessage = "CRP required 8 characters")]
    [MaxLength(8, ErrorMessage = "CRP required 8 characters")]
    public string CRP { get; set; } = string.Empty;

    [Required(ErrorMessage = "Specialization is required")]
    [MaxLength(50, ErrorMessage = "Specialization must be at most 50 characters long")]
    public string Specialization { get; set; } = string.Empty;
}
