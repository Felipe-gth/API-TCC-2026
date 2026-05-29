namespace Api.Admin.Interfaces;
using Api.Admin.DTOs.Register;

public interface IAdmInterface
{
    Task<bool> CreateAdmin(RegisterAdminDTO admin);
}