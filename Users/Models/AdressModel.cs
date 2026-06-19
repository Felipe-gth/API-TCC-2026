using Api.User.DTOs.Adress;

namespace Api.User.Models;


public class AdressModel
{
    public int Id { get; set; }
    public string CEP { get; private set; }
    public string CCity { get; private set; }
    public string CState { get; private set; }
    public string CStreet { get; private set; }
    public string CNumber { get; private set; }
    public string CNieghBorhood { get; private set; }
    public bool IsApartment { get; private set; }
    public int Floor { get; set; }
    public int ApartmentNumber { get; set; }
    public int PatientId { get; set; }


    public AdressModel(){}
    public AdressModel(AdressEntryDTO dto)
    {
        CEP = dto.CEP;
        CCity = dto.City;
        CState = dto.State;
        CStreet = dto.Street;
        CNumber = dto.Number;
        CNieghBorhood = dto.Neighborhood;
        IsApartment = dto.IsApartment;
        Floor = dto.Floor;
        ApartmentNumber = dto.ApartmentNumber;
        PatientId = dto.PatientId;
    }
}