namespace Api.Patient.Models;

public class ReportModel
{
    public int Id {get; private set;}
    public string EmotinalState {get; private set;}
    public int AppointmentId {get; private set;}

    public ReportModel(int id, string emotinalState, int appointmentId)
    {
        Id = id;
        EmotinalState = emotinalState;
        AppointmentId = appointmentId;
    }
}