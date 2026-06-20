namespace Api.Patient.Models;


public class IMRModel
{
    public int Id { get; private set; }
    public bool HadTreatment { get; private set; }
    public string PhysicalHealth { get; private set; }
    public char MaritalStatus { get; private set; }
    public string Habits { get; private set; }
    public string SearchReason { get; private set; }

    public IMRModel(int id, bool hadTreatment, string physicalHealth, char maritalStatus, string habits, string searchReason)
    {
        Id = id;
        HadTreatment = hadTreatment;
        PhysicalHealth = physicalHealth;
        MaritalStatus = maritalStatus;
        Habits = habits;
        SearchReason = searchReason;
    }

}