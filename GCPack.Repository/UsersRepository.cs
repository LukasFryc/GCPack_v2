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
                db.Logins.RemoveRange(db.Logins.Where (l => l.UserID == user.ID));
                db.SaveChanges();
                db.Logins.Add(new Login() {
                    LastTick = DateTime.Now,
                    Hash = ticket,
                    UserID = user.ID
                });
                db.SaveChanges();
            }
        }

    }
}
