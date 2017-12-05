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
        readonly ILogEventsService logEventsService;

        // IOC  se inicializuje v boostrap (ten je GCPack.Web/App_Start)
        public UsersService(IUsersRepository usersRepository, ILogEventsService logEventsService)
        {
            this.usersRepository = usersRepository;
            this.logEventsService = logEventsService;
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
            // po prihlaseni pokud je nacteny uzivatel, tak zaloguju ID uzivatele, UserLogin - udalost, 0 = nejedna se o zadny zdroj 
            // se kterym se pracovalo (tj. dokument nebo uzivatel)
            // ok ? ok
            // jen bych chtel obcas logovat i nejake vlastni texty napr 
            // duvod storna
            // kam to dat

            if (user != null) logEventsService.LogEvent(user.ID, LogEventType.UserLogin, 0);

            return ticket;
        }

        public ICollection<JobPositionModel> GetUserJobPositions(int userID)
        {
            return usersRepository.GetUserJobPositions(userID);
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

            UserFilter filter = new UserFilter();

            HashSet<int> UserIDs = new HashSet<int>();
            UserIDs.Add(user.ID);
            filter.UserIDs = UserIDs;
            UserJobCollectionModel usersJobs = GetUsersJob(filter);

            HashSet<int> JobPositionIDAdds = new HashSet<int>();


            foreach (var item in user.JobPositionIDs) {
                
                if (!usersJobs.UserJobs.Select(uj=>uj.JobPositionID).Contains(item))
                {
                    JobPositionIDAdds.Add(item);
                }
            }

            UserModel userSave = usersRepository.SaveUser(user);

            AddUserToReadConfirms(user, JobPositionIDAdds);

            return userSave;


        }

        public UserModel GetUser(int userID)
        {
            var user = usersRepository.GetUser(userID);
            user.JobPositions = usersRepository.GetJobPositionIDs(userID);
            //user.JobPositionsAll = usersRepository.GetJobPositionsUser(userID);
            user.JobPositions = (user.JobPositions == null) ? new HashSet<int>() : user.JobPositions;
            //user.JobPositionsName = 

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

        //public JobPositionModel GetJobPosition(int ID)
        //{
        //    return usersRepository.GetJobPosition(ID);
        //}

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
            ICollection <UserModel> users = usersRepository.GetUsers(filter);
            foreach (var user in users)
            {
                user.Password = "";
            }
            return users;
        }

        public UserModel AddUser(UserModel user)
        {
            UserModel userAdd =  usersRepository.AddUser(user);

            return userAdd;

        }

        public void DeleteUser(int userId)
        {
            usersRepository.DeleteUser(userId);
        }

        public UserJobCollectionModel GetUsersJob(UserFilter filter)
        {
            return usersRepository.GetUsersJob(filter);
        }

        public void AddUserToReadConfirms(UserModel user, ICollection<int> JobPositionIDAdds)
        {
            usersRepository.AddUserToReadConfirms(user, JobPositionIDAdds);
        }

    }
}
