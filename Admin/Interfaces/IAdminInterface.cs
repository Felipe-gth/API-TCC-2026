namespace Api.Admin.Interfaces;
using Api.Admin.DTOs.Register;

public interface IAdminInterface
{
    Task<bool> CreateAdminAsync(RegisterAdminDTO admin);
}