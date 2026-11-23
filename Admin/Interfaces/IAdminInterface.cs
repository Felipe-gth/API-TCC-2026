namespace Api.Admin.Interfaces;
using Api.Admin.DTOs.Register;
using Api.Shared.DTOs.Result;

public interface IAdminInterface
{
    Task<Result<bool>> CreateAdminAsync(RegisterAdminDTO admin);
}