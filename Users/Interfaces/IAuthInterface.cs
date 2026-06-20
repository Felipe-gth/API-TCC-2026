
namespace Api.User.Interfaces;
public interface IAuthInterface
{
    string NewToken(int id, string role);
}