namespace Api.Patient.Models;


public class IMRModel
{
    public int Id { get; private set; }
    public bool HadTreatment { get; private set; }
    public string PhysicalHeath { get; private set; }
    public char MaritalStatus { get; private set; }
    public string Habits { get; private set; }
    public string Searchreason { get; private set; }

    public IMRModel(int id, bool hadTreatment, string physicalHeath, char maritalStatus, string habits, string searchreason)
    {
        Id = id;
        HadTreatment = hadTreatment;
        PhysicalHeath = physicalHeath;
        MaritalStatus = maritalStatus;
        Habits = habits;
        Searchreason = searchreason;
    }

}