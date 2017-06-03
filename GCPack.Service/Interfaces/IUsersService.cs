using GCPack.Model;
using System.Collections.Generic;

namespace GCPack.Service.Interfaces
{
    public interface IUsersService
    {
        UserModel GetUser(string ticket);
        string Login(string username, string password);

        ICollection<UserModel> GetUsers(UserFilter filter);
        ICollection<JobPositionModel> GetJobPositions();
        UserModel AddUser(UserModel user);
        void DeleteUser(int userId);
    }
}