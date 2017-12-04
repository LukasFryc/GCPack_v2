using GCPack.Model;
using System.Collections.Generic;
using System.Linq;

namespace GCPack.Repository.Interfaces
{
    public interface IUsersRepository
    {
        UserModel GetUser(string username, string password);
        void UpdateTicket(string ticket, UserModel user);
        UserModel GetUser(string ticket);
        ICollection<RoleModel> GetRoles();
        ICollection<JobPositionModel> GetJobPositions();
        ICollection<UserModel> GetUsers(UserFilter filter);
        void DeleteUser(int userId);
        UserModel AddUser(UserModel user);
        UserModel GetUser(int userID);
        UserModel SaveUser(UserModel user);
        ICollection<int> GetJobPositionIDs(int userID);
        //JobPositionModel GetJobPosition(int ID);
        ICollection<JobPositionModel> GetUserJobPositions(int userID);
        ICollection<UserJobModel> GetUsersJob(UserFilter filter);
        void AddUserToReadConfirms(UserModel user, ICollection<int> JobPositionIDAdds);
    }
}