namespace Api.Appointment.DTOs.List;

public class ServiceDayHourDTO
{
    public int WeekDay { get; set; }
    public string Hour { get; set; } = string.Empty;
}

public class ServiceDayDTO
{
    public int WeekDay { get; set; }
    public List<string> Hours { get; set; } = new();
}
