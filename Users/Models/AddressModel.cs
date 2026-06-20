using Api.User.DTOs.Address;

namespace Api.User.Models;


public class AddressModel
{
    public int Id { get; set; }
    public string CEP { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string Street { get; private set; }
    public string Number { get; private set; }
    public string Neighborhood { get; private set; }
    public bool IsApartment { get; private set; }
    public int Floor { get; set; }
    public int ApartmentNumber { get; set; }
    public int PatientId { get; set; }


    public AddressModel(){}
    public AddressModel(AddressEntryDTO dto)
    {
        CEP = dto.CEP;
        City = dto.City;
        State = dto.State;
        Street = dto.Street;
        Number = dto.Number;
        Neighborhood = dto.Neighborhood;
        IsApartment = dto.IsApartment;
        Floor = dto.Floor;
        ApartmentNumber = dto.ApartmentNumber;
        PatientId = dto.PatientId;
    }
}