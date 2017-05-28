using GCPack.Model;

namespace GCPack.Service.Interfaces
{
    public interface IUsersService
    {
        UserModel GetUser(string ticket);
        string Login(string username, string password);
    }
}