namespace Api.Shared.DTOs.Result;

public class Result<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
}
