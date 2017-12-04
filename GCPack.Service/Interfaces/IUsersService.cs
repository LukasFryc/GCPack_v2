using GCPack.Model;
using System.Collections.Generic;
using System.Linq;

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
        ICollection<JobPositionModel> GetUserJobPositions(int userID);
        //JobPositionModel GetJobPosition(int ID);

        ICollection<UserJobModel> GetUsersJob(UserFilter filter);

        void AddUserToReadConfirms(UserModel user, ICollection<int> JobPositionIDAdds);

    }
}