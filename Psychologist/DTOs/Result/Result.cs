namespace Api.User.DTOs.Result;

public class Result<T>
{
    public bool Sucess { get; set; }
    public T? Data { get; set; }
}