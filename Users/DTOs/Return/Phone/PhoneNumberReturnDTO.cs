namespace Api.User.DTOs.Return.Phone;
public class PhoneNumberReturnDTO{
    public int Id {get; set;}
    public string Number {get; set;}
    public string DDD {get; set;}
    public PhoneNumberReturnDTO(int id, string number, string ddd)
    {
        Id = id;
        DDD = ddd;
        Number = number;

    }
}