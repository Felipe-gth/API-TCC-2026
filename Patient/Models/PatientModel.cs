namespace Api.Patient.Models;

using Api.User.Models;

public class PatientModel : UserModel{
    public int PatientId {get; private set;}
    public PatientModel()
    {
        
    }
    public PatientModel(int id, string name, string lastName, string cpf, string age, string password, string role = "C") : base(id, name, lastName, cpf, age, password, role){
    }
}