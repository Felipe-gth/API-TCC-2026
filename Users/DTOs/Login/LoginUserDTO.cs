namespace Api.User.DTOs.Login;
using System.ComponentModel.DataAnnotations;


public class LoginUserDTO{
    [StringLength(11, MinimumLength = 11)]
    [Required(ErrorMessage = "CPF is required")]
    public string CPF {get; set;}
    [Required(ErrorMessage = "Password is required")]
    public string Password {get; set;}
}