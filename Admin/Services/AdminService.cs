namespace Api.Admin.Services;
using Api.Admin.Interfaces;
using Api.Admin.DTOs.Register;
using Api.Admin.Models;
using Api.Admin.Data.InterfaceSql;
public class AdminService : IAdmInterface
{
    private readonly IAdminInterfaceSql _adminInterfaceSql;

    public AdminService(IAdminInterfaceSql adminInterfaceSql)
    {
        _adminInterfaceSql = adminInterfaceSql;
    }
    public async Task<bool> CreateAdmin(RegisterAdminDTO admin)
    {
        var adminModel = new AdminModel(admin.Name, admin.LastName, admin.CPF, admin.Password, admin.Role);
        var id = await _adminInterfaceSql.CreateAdminAsync(adminModel);
        if(id > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}