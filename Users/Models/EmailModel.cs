namespace Api.User.Models;


public class EmailModel
{
    public int id { get; private set; }
    public string EAddress { get; set; }
    public string EExtension { get; set; }

    public EmailModel(int id, string eAddress, string eExtension)
    {
        this.id = id;
        EAddress = eAddress;
        EExtension = eExtension;
    }
}