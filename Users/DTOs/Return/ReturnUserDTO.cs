namespace Api.User.DTOs.Return;
public class ReturnUserDTO{
    public int Id {get; set;}
    public string Token { get; set; }
    public string Role {get; set;}
    public ReturnUserDTO(int id, string token, string role){
        Id = id;
        Token = token;
        Role = role;
    }
}