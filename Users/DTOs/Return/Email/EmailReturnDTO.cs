namespace Api.User.DTOs.Return.Email;
public class EmailReturnDTO{
    public int Id {get; set;}
    public string Address {get; set;}
    public string Extension {get; set;}
    
    public EmailReturnDTO(int id, string address, string extension){
        Id = id;
        Address = address;
        Extension = extension;
    }
}