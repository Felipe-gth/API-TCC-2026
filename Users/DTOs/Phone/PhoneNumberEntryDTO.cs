namespace Api.User.DTOs.Phone;
using System.ComponentModel.DataAnnotations;

public class PhoneNumberEntryDTO{
    [Required(ErrorMessage = "Id is required.")]
    public int Id {get; set;}
    [Required(ErrorMessage = "Number is required.")]
    public string Number {get; set;}
    [Required(ErrorMessage = "IsEmergencyContact is required.")]
    public bool IsEmergencyContact {get; set;}
    [Required(ErrorMessage = "CountryCode is required.")]
    public string CountryCode {get; set;}
    [Required(ErrorMessage = "DDD is required.")]
    public string DDD {get; set;}

}