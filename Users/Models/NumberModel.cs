namespace Api.User.Models;
public class NumberModel
{
    public int Id { get; private set; }
    public string NNumber { get;  private set; }
    public string NCountryCode { get;  private set; }
    public string NDDD { get;  private set; }
    public bool IsEmergencyContact { get;  private set; }

    public NumberModel(int id, string nNumber, string nCountryCode, string nDDD, bool isEmergencyContact)
    {
        Id = id;
        NNumber = nNumber;
        NCountryCode = nCountryCode;
        NDDD = nDDD;
        IsEmergencyContact = isEmergencyContact;
    }
}