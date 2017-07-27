using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCPack.Repository.Interfaces;
using GCPack.Model;
using AutoMapper;

namespace GCPack.Repository
{
    public class UsersRepository : IUsersRepository
    {
        public UserModel GetUser(string username, string password)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                //ahoj
                return Mapper.Map<UserModel>(db.Users.Where(u => u.UserName == username && u.Password == password).Select(u => u).SingleOrDefault());
            }
        }

        public UserModel CheckTicket(string ticket)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                var user = (from u in db.Users
                           from l in db.Logins
                           where u.ID == l.UserID && l.Hash == ticket
                           select u).FirstOrDefault();
                if (user != null)
                {
                    user.Logins.FirstOrDefault().LastTick = DateTime.Now;
                    db.SaveChanges();
                }

                return Mapper.Map<UserModel>(user);
            }
        }

        public UserModel GetUser(int userID)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                var us = (from u in db.Users
                          where u.ID == userID
                          select u).FirstOrDefault();

                UserModel user = Mapper.Map<UserModel>(us);
                user.RoleIDs = us.UserRoles.Select(ur => ur.RoleId).ToList<short>();
                return user;
            }
        }

        public ICollection<int> GetJobPositionIDs(int userID)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                var jobPositionIDs = (from u in db.JobPositionUsers
                          where u.UserId == userID
                          select u.JobPositionId).ToList();
                return jobPositionIDs;
            }
        }

        public UserModel GetUser(string ticket)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                var us = (from u in db.Users
                        from l in db.Logins
                        where u.ID == l.UserID && l.Hash == ticket
                        select u).FirstOrDefault();

                UserModel user = Mapper.Map<UserModel>(us);

                if (user != null)
                {
                    user.Roles = "";
                    foreach (var role in us.UserRoles)
                    {
                        user.Roles += role.Role.RoleCode + ",";
                    }
                    user.Roles = user.Roles.TrimEnd(',');
                }
                

                return user;
            }
        }

        public void Logout(UserModel user)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                db.Logins.RemoveRange(db.Logins.Where(l => l.UserID == user.ID));
                db.SaveChanges();
            }
        }

        public void UpdateTicket(string ticket, UserModel user)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                if (db.Logins.Where(l => l.UserID == user.ID).Count() > 0)
                {
                    db.Logins.RemoveRange(db.Logins.Where(l => l.UserID == user.ID));
                    db.SaveChanges();
                }

                db.Logins.Add(new Login() {
                    LastTick = DateTime.Now,
                    Hash = ticket,
                    UserID = user.ID
                });
                db.SaveChanges();
            }
        }

        public ICollection<JobPositionModel> GetJobPositions()
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                return Mapper.Map<ICollection<JobPositionModel>>(db.JobPositions.Select(jp => jp));
            }
        }

        public ICollection<RoleModel> GetRoles()
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                var roles = db.Roles.Select(r => r).ToList();
                return Mapper.Map<ICollection<RoleModel>>(roles);
            }
        }

        public UserModel SaveUser(UserModel user)
        {

            using (GCPackContainer db = new GCPackContainer())
            {
                if (user.ID != 0)
                {
                    db.JobPositionUsers.RemoveRange(db.JobPositionUsers.Where(jpu => jpu.User.ID == user.ID));
                    db.SaveChanges();
                    if (user.JobPositionIDs != null)
                    {
                        foreach (var jobID in user.JobPositionIDs)
                        {
                            db.JobPositionUsers.Add(new JobPositionUser() { UserId = user.ID, JobPositionId = jobID, Created = DateTime.Now });
                        }

                        db.SaveChanges();
                    }

                    db.UserRoles.RemoveRange(db.UserRoles.Where(ur => ur.UserID == user.ID));
                    db.SaveChanges();
                    foreach (short roleId in user.RoleIDs)
                    {
                        db.UserRoles.Add(new UserRole() { UserID = user.ID, RoleId = roleId });
                    }
                    db.SaveChanges();

                    var dbUser = db.Users.Where(u => u.ID == user.ID).Select(u => u).FirstOrDefault();
                    Mapper.Map(user, dbUser);
                    dbUser.UserNumber = (dbUser.UserNumber == null) ? string.Empty : dbUser.UserNumber;
                    db.SaveChanges();
                }
                else
                {
                    var dbUser = db.Users.Add(Mapper.Map<User>(user));
                    dbUser.UserNumber = (dbUser.UserNumber == null) ? string.Empty : dbUser.UserNumber;
                    db.SaveChanges();
                    user.ID = dbUser.ID;
                }



            }

            return user;
        }
            public ICollection<UserModel> GetUsers(UserFilter filter)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                //var users = db.Users.Where(u => 
                //    ((u.LastName.ToLower().Contains(filter.Name) || (u.FirstName.ToLower().Contains(filter.Name) || filter.Name == null)) &&
                //    (!filter.ExcludedUsersId.Contains(u.ID))
                //)).Select(u => u);

                var jpus = from jpu in db.JobPositionUsers where filter.JobPositionIDs.Contains(jpu.JobPositionId) select jpu;

                var users = from u in db.Users
                        where
                            (
                                (u.LastName.ToLower().Contains(filter.Name) || (u.FirstName.ToLower().Contains(filter.Name) || string.IsNullOrEmpty (filter.Name))) &&
                                (!filter.ExcludedUsersId.Contains(u.ID))  &&
                                (filter.JobPositionIDs.FirstOrDefault() == 0 || jpus.Select(j => j.UserId).Contains(u.ID))
                            )
                        select u;

                switch (filter.OrderBy)
                {

                    case "FirstName":
                        users = users.OrderBy(u => u.FirstName).ThenBy(u => u.LastName);
                        break;
                    case "LastName":
                        users = users.OrderBy(u => u.LastName).ThenBy(u => u.FirstName);
                        break;
                }

                foreach (User user in users)
                {
                    string xx = user.FirstName;
                }

                return Mapper.Map<ICollection<UserModel>>(users);
            }
        }

        public void DeleteUser(int userId)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                db.Logins.RemoveRange(db.Logins.Where(l => l.UserID == userId));
                db.SaveChanges();
                var user = db.Users.Where(u => u.ID == userId).SingleOrDefault();
                if (user != null) db.Users.Remove(user);
                db.SaveChanges();
            }
        }

        public UserModel AddUser(UserModel user)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                User newDbUser = db.Users.Add(Mapper.Map<User>(user));
                db.SaveChanges();
                // bacha toto je finta tyv reveryni mapovani - vracim si z db zpatky model
                return Mapper.Map<UserModel>(newDbUser);
            }
        }

    }
}
