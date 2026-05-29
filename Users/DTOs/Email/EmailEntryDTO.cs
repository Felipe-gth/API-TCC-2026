namespace Api.User.DTOs.Email;
using System.ComponentModel.DataAnnotations;
public class EmailEntryDTO{
    [Required(ErrorMessage = "Id is required.")]
    public int Id {get; set;}

    [Required(ErrorMessage = "Email is required.")]
    public string Address {get; set;}

    [Required(ErrorMessage = "Extension is required.")] 
    public string Extension {get; set;}

}