namespace Api.Patient.Models;

public class ReportModel
{
    public int Id {get; private set;}
    public string EmotionalState {get; private set;}
    public int AppointmentId {get; private set;}

    public ReportModel(int id, string emotionalState, int appointmentId)
    {
        Id = id;
        EmotionalState = emotionalState;
        AppointmentId = appointmentId;
    }
}