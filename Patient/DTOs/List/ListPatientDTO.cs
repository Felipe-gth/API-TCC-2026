namespace Api.Patient.DTOs.List;

public class ListPatientDTO
{
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string Age { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    public ListPatientDTO() {}
}