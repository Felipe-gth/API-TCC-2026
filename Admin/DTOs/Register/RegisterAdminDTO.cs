using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Admin.DTOs.Register;

public class RegisterAdminDTO
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 100 caracteres.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "O sobrenome é obrigatório.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O sobrenome deve ter entre 2 e 100 caracteres.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "O CPF é obrigatório.")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve conter 11 dígitos.")]
    public string CPF { get; set; }


    [Required(ErrorMessage = "A senha é obrigatória.")]
    [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "O cargo é obrigatório.")]
    [RegularExpression("A", ErrorMessage = "O cargo deve ser 'A' para administrador.")]
    public string Role { get; set; }


}