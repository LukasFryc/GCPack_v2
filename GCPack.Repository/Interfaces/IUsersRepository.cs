using GCPack.Model;
using System.Collections.Generic;
namespace GCPack.Repository.Interfaces
{
    public interface IUsersRepository
    {
        UserModel GetUser(string username, string password);
        void UpdateTicket(string ticket, UserModel user);
        UserModel GetUser(string ticket);
        ICollection<JobPositionModel> GetJobPositions();
        ICollection<UserModel> GetUsers(UserFilter filter);
        void DeleteUser(int userId);
        UserModel AddUser(UserModel user);

    }
}