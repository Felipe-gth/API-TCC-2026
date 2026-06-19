
namespace Api.User.Interfaces;
public interface IAuthInterface
{
    string newToken(int id, string role);
}