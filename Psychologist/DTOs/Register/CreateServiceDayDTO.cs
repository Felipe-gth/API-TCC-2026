namespace Api.Psychologist.DTOs.Register;
using System.ComponentModel.DataAnnotations;

public class CreateServiceDayDTO
{
    [Range(0, 6, ErrorMessage = "WeekDay must be between 0 and 6")]
    public int WeekDay { get; set; }

    [Required(ErrorMessage = "Hours is required")]
    [MinLength(1, ErrorMessage = "At least one hour must be informed")]
    public List<string> Hours { get; set; } = new();
}