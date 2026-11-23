namespace Api.Admin.Services;
using Api.Admin.Interfaces;
using Api.Admin.DTOs.Register;
using Api.Admin.Models;
using Api.Admin.Data.InterfaceSql;
using Api.Shared.DTOs.Result;
public class AdminService : IAdminInterface
{
    private readonly IAdminInterfaceSql _adminInterfaceSql;

    public AdminService(IAdminInterfaceSql adminInterfaceSql)
    {
        _adminInterfaceSql = adminInterfaceSql;
    }
    public async Task<Result<bool>> CreateAdminAsync(RegisterAdminDTO admin)
    {
        var adminModel = new AdminModel(admin.Name, admin.LastName, admin.CPF, admin.Password, admin.Role);
        var id = await _adminInterfaceSql.CreateAdminAsync(adminModel);
        if (id > 0)
        {
            return new Result<bool>(true, true);
        }
        if (id == -1)
        {
            return new Result<bool>(false, false) { Message = "CPF ja cadastrado." };
        }
        return new Result<bool>(false, false);
    }
}