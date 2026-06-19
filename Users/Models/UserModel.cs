namespace Api.User.Models;


public class UserModel{
    public int Id { get;  set; }
    public string Name {get;  set;}
    public string LastName {get;  set;}
    public string CPF {get;  set;}
    public string Age {get;  set;}
    public string Password {get;  set;}
    public string Role {get;  set;}

    public UserModel(){
    }

    public UserModel(int id, string name, string lastName, string cpf, string age, string password, string role){
        Id = id;
        Name = name;
        LastName = lastName;
        CPF = cpf;
        Age = age;
        Password = password;
        Role = role;
    }
}