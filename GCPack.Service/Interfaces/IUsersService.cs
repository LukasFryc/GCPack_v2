using GCPack.Model;
using System.Collections.Generic;

namespace GCPack.Service.Interfaces
{
    public interface IUsersService
    {
        UserModel GetUser(string ticket);
        string Login(string username, string password);

        ICollection<UserModel> GetUsers(UserFilter filter);
        ICollection<Item> GetUserList(UserFilter filter);
        ICollection<Item> GetRoles();
        ICollection<JobPositionModel> GetJobPositions();
        UserModel AddUser(UserModel user);
        UserModel GetUser(int userId);
        UserModel SaveUser(UserModel user);
        void DeleteUser(int userId);
    }
}