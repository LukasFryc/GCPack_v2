using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCPack.Model;
using GCPack.Repository.Interfaces;
using AutoMapper;
using GCPack.Service.Interfaces;

namespace GCPack.Service
{
    public class UsersService : IUsersService
    {
        readonly IUsersRepository usersRepository;

        // IOC  se inicializuje v boostrap (ten je GCPack.Web/App_Start)
        public UsersService(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public string Login(string username, string password)
        {
            // pokud se uzivatel prihlasi, pak se prida ticket do databaze
            string ticket = "";
            UserModel user = usersRepository.GetUser(username, password);
            if (user != null)
            {
                ticket = Guid.NewGuid().ToString();
                usersRepository.UpdateTicket(ticket, user);
            }
            return ticket;
        }

        public UserModel GetUser(string ticket)
        {
            var user = usersRepository.GetUser(ticket);
            return user;
        }

        public UserModel SaveUser(UserModel user)
        {
            // TODO: zjisti se vsechny nove pracovni pozice
            // do kterych byl uzivatel prirazen a pokud jsou nektere
            // nove, tak se odesle email

            return usersRepository.SaveUser(user);
        }

        public UserModel GetUser(int userID)
        {
            var user = usersRepository.GetUser(userID);
            user.JobPositions = usersRepository.GetJobPositionIDs(userID);
            user.JobPositions = (user.JobPositions == null) ? new HashSet<int>() : user.JobPositions;
            return user;
        }

        public void UpdateTicket(string ticket, UserModel user)
        {
            usersRepository.UpdateTicket(ticket,user);
        }

        public ICollection<JobPositionModel> GetJobPositions()
        {
            return usersRepository.GetJobPositions();
        }

        // vraci se jednoducha kolekce uzivatelu - ID , Value
        public ICollection<Item> GetUserList(UserFilter filter)
        {
            return Mapper.Map<ICollection<Item>>(GetUsers(filter));
        }

        public ICollection<Item> GetRoles()
        {
            return Mapper.Map<ICollection<Item>>(usersRepository.GetRoles());
        }

        public ICollection<UserModel> GetUsers(UserFilter filter)
        {
            return usersRepository.GetUsers(filter);
        }

        public UserModel AddUser(UserModel user)
        {
            return usersRepository.AddUser(user);
        }

        public void DeleteUser(int userId)
        {
            usersRepository.DeleteUser(userId);
        }

    }
}
