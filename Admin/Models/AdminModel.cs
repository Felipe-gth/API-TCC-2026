namespace Api.Admin.Models;

public class AdminModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string LastName { get; set; }

    public string CPF { get; set; }


    public string Password { get; set; }

    public string Role { get; set; }


    public AdminModel() { }

    public AdminModel(
        string name,
        string lastName,
        string cpf,
        string password,
        string role
    )
    {
        Name = name;
        LastName = lastName;
        CPF = cpf;
        Password = password;
        Role = role;
    }
}