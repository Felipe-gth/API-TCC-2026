namespace Api.User.Models;
public class NumberModel
{
    public int Id { get; private set; }
    public string Number { get;  private set; }
    public string CountryCode { get;  private set; }
    public string DDD { get;  private set; }
    public bool IsEmergencyContact { get;  private set; }

    public NumberModel(int id, string number, string countryCode, string ddd, bool isEmergencyContact)
    {
        Id = id;
        Number = number;
        CountryCode = countryCode;
        DDD = ddd;
        IsEmergencyContact = isEmergencyContact;
    }
}