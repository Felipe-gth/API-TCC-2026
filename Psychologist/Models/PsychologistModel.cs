namespace Api.Psychologist.Models;

using Api.User.Models;

public class PsychologistModel : UserModel{
    public string CRP {get; private set;}
    public string Specialization {get; private set;}
    public int PsychologistId {get; private set;}
    public PsychologistModel(){
        
    }
    public PsychologistModel(int id, string name, string lastName, string cpf, string age, string password, string specialization, string crp) : base(id, name, lastName, cpf, age, password, "P"){
        CRP = crp;
        Specialization = specialization;
    }
}