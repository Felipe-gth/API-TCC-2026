namespace Api.User.DTOs.Adress;
using System.ComponentModel.DataAnnotations;


public class AdressEntryDTO{

    [Required(ErrorMessage = "Id is required.")]
    public int Id {get; set;}
    
    [Required(ErrorMessage = "CEP is required.")]
    public string CEP {get; set;}

    [Required(ErrorMessage = "City is required.")]
    public string City {get; set;}

    [Required(ErrorMessage = "State is required.")]
    public string State {get; set;}

    [Required(ErrorMessage = "Street is required.")]
    public string Street {get; set;}

    [Required(ErrorMessage = "Number is required.")]
    public string Number {get; set;}

    [Required(ErrorMessage = "Neighborhood is required.")]
    public string Neighborhood {get; set;}

    [Required(ErrorMessage = "Complement is required.")]
    public bool IsApartment {get; set;}

    [Required(ErrorMessage = "Floor is required.")]
    public int Floor {get; set;}

    [Required(ErrorMessage = "Apartment Number is required.")]
    public int ApartmentNumber {get; set;}
    [Required(ErrorMessage = "PatientId is required.")]
    public int PatientId {get; set;}

    

}