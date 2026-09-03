namespace Api.Shared.DTOs.Result;

public class Result<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }

    public Result() { }

    public Result(bool success, T? data)
    {
        Success = success;
        Data = data;
    }
}
