namespace Api.Admin.Data.InterfaceSql;
using Api.Admin.Models;
public interface IAdminInterfaceSql
{
    Task<int> CreateAdminAsync(AdminModel admin);
}