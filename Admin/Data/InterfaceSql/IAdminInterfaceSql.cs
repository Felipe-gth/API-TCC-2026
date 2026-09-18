namespace Api.Admin.Data.InterfaceSql;
using Api.Admin.Models;
using Api.Admin.DTOs.Return;
public interface IAdminInterfaceSql
{
    Task<int> CreateAdminAsync(AdminModel admin);
    Task<ReturnAdminData> GetAdminByIdAsync(int id);
    
}