using GCPack.Model;
namespace GCPack.Repository.Interfaces
{
    public interface IUsersRepository
    {
        UserModel GetUser(string username, string password);
        void UpdateTicket(string ticket, UserModel user);
        UserModel GetUser(string ticket);
    }
}