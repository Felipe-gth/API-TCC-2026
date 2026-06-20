namespace Api.User.Models;


public class EmailModel
{
    public int Id { get; private set; }
    public string Address { get; set; }
    public string Extension { get; set; }

    public EmailModel(int id, string address, string extension)
    {
        Id = id;
        Address = address;
        Extension = extension;
    }
}