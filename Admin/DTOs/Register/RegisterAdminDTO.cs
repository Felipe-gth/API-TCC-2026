using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Admin.DTOs.Register;

public class RegisterAdminDTO
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 100 characters.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "CPF is required.")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF must have 11 digits.")]
    public string CPF { get; set; }


    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Role is required.")]
    [RegularExpression("A", ErrorMessage = "Role must be 'A' for admin.")]
    public string Role { get; set; }


}