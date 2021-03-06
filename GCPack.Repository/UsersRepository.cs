﻿using System;
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
                // LF doplneno 9.11.2017 && u.Active == true
                return Mapper.Map<UserModel>(db.Users.Where(u => u.UserName == username && u.Password == password && u.Active == true).Select(u => u).SingleOrDefault());
            }
        }

        public UserModel CheckTicket(string ticket)
        {
            // LF doplneno 9.11.2017 && u.Active == true
            using (GCPackContainer db = new GCPackContainer())
            {
                var user = (from u in db.Users
                           from l in db.Logins
                           where u.ID == l.UserID && l.Hash == ticket && u.Active == true
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
                user.Roles = us.UserRoles.Select(ur => ur.Role.RoleCode).FirstOrDefault();
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

        //LF 1.11.2017
        public ICollection<JobPositionUser> GetJobPositionsUser(int userID)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                var jobPositionUsers = (from u in db.JobPositionUsers
                                      where u.UserId == userID
                                      select u);
                
                return Mapper.Map<ICollection<JobPositionUser>>(jobPositionUsers);
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

        //LF 1.11.2017 doplneno pro navrat konkretni pracovni pozice
        //public JobPositionModel GetJobPosition(int ID)
        //{
        //    using (GCPackContainer db = new GCPackContainer())
        //    {
        //        return Mapper.Map<JobPositionModel>(db.JobPositions.Select(jp => jp).Where(jp=>jp.ID==ID).FirstOrDefault());
        //    }
        //}

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
                user.UserNumber = (user.UserNumber == null) ? string.Empty : user.UserNumber;

                if (user.ID != 0)
                {
                    db.JobPositionUsers.RemoveRange(db.JobPositionUsers.Where(jpu => jpu.User.ID == user.ID));
                    db.SaveChanges();
                    
                    db.UserRoles.RemoveRange(db.UserRoles.Where(ur => ur.UserID == user.ID));
                    db.SaveChanges();
                    
                    var dbUser = db.Users.Where(u => u.ID == user.ID).Select(u => u).FirstOrDefault();
                    Mapper.Map(user, dbUser);
                    db.SaveChanges();
                }
                else
                {
                    var dbUser = db.Users.Add(Mapper.Map<User>(user));
                    db.SaveChanges();
                    user.ID = dbUser.ID;
                }

                foreach (short roleId in user.RoleIDs)
                {
                    db.UserRoles.Add(new UserRole() { UserID = user.ID, RoleId = roleId });
                }
                db.SaveChanges();

                if (user.JobPositionIDs != null)
                {
                    foreach (var jobID in user.JobPositionIDs)
                    {
                        db.JobPositionUsers.Add(new JobPositionUser() { UserId = user.ID, JobPositionId = jobID, Created = DateTime.Now });
                    }

                    db.SaveChanges();
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
                                //(u.LastName.ToLower().Contains(filter.Name) || (u.FirstName.ToLower().Contains(filter.Name) || string.IsNullOrEmpty (filter.Name))) &&
                                //(!filter.ExcludedUsersId.Contains(u.ID))  &&
                                //(filter.JobPositionIDs.FirstOrDefault() == 0 || jpus.Select(j => j.UserId).Contains(u.ID))

                                (u.LastName.ToLower().Contains(filter.Name) || (u.FirstName.ToLower().Contains(filter.Name) || string.IsNullOrEmpty(filter.Name))) &&
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

        public ICollection<JobPositionModel> GetUserJobPositions(int userID)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                return Mapper.Map<ICollection<JobPositionModel>>(db.JobPositionUsers.Where(jpu => jpu.UserId == userID).Select(jpu => jpu.JobPosition));
            }
        }

        public UserJobCollectionModel GetUsersJob(UserFilter filter)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                //var dbUsers = db.Users.Where(u => filter.UserIDs.Contains(u.ID)).Select(u=>u);

                var dbJobUsersPositions = db.JobPositionUsers.Where(jpu => filter.JobPositionIDs.Contains(jpu.JobPositionId) || filter.UserIDs.Contains(jpu.UserId)).Select(jpu => jpu);
                //var dbJobUsersPositions = db.JobPositionUsers.Select(jpu => jpu).Where(jpu => filter.JobPositionIDs.Contains(jpu.JobPositionId));

                List<UserJobModel> UserJobs = new List<UserJobModel>();

                UserJobCollectionModel UserJobCollection = new UserJobCollectionModel();

                UserJobCollection.UserJobs = null;

                if (dbJobUsersPositions != null || dbJobUsersPositions.Count() > 0)
                {

                    foreach (var item in dbJobUsersPositions)
                    {
                        UserJobModel userJob = new UserJobModel();
                        userJob.FirstName = item.User.FirstName;
                        userJob.LastName = item.User.LastName;
                        userJob.JobPositionID = item.JobPositionId;
                        userJob.JobPositionName = item.JobPosition.Name;
                        userJob.UserID = item.UserId;

                        UserJobs.Add(userJob);
                    }

                    switch (filter.OrderBy)
                    {
                        case "GetUsersJob_NameA":
                            UserJobCollection.UserJobs = UserJobs.AsQueryable().OrderBy(uj => uj.LastName).ToList();
                            break;
                        case "GetUsersJob_NameD":
                            UserJobCollection.UserJobs = UserJobs.AsQueryable().OrderByDescending(uj => uj.LastName).ToList();
                            break;
                        case "GetUsersJob_JobPostionA":

                            UserJobCollection.UserJobs = UserJobs.AsQueryable().OrderBy(uj => uj.JobPositionName).ToList();
                            break;
                        case "GetUsersJob_JobPostionD":
                            UserJobCollection.UserJobs = UserJobs.AsQueryable().OrderByDescending(uj => uj.JobPositionName).ToList();
                            break;
                        default:
                            // pro pripad ze nebude vyplneno order by
                            // coz se skutecne deje pokud funkce neni volana y view pres json, ale pri zaevidovani
                            // pak to logicky padalo
                            UserJobCollection.UserJobs = UserJobs.AsQueryable().ToList();
                            break;
                    }

                }
                

                UserJobCollection.filter = filter;

                return UserJobCollection;

            }
        }

        // ne void ale icollection<RedadconfirmModel>
        public void AddUserToReadConfirms(UserModel user, ICollection<int> JobPositionIDAdds)
        {
            using (GCPackContainer db = new GCPackContainer())
            {

                //var dbJpu = db.JobPositionUsers.Where(jpu => user.JobPositionIDs.Contains(jpu.JobPositionId)).Select(jpu => jpu);

                //ICollection<int> jp = new HashSet<int>();

                //foreach (var jpID in user.JobPositionIDs)
                //{
                //    var pom = dbJpu.Where(jpu => jpu.JobPositionId == jpID).Count();
                //    if (pom == 0)
                //    {
                //        jp.Add(jpID);
                //    }
                //}

                var dbJpd = db.JobPositionDocuments.Where(jpd => JobPositionIDAdds.Contains(jpd.JobPositionId)).Select(jpd => jpd);

                foreach(var item in dbJpd)
                {
                    ReadConfirmation readConfirm = new ReadConfirmation();

                    readConfirm.DocumentID = item.DocumentId;
                    readConfirm.JobPositionID = item.JobPositionId;
                    readConfirm.JobPositionName = item.JobPosition.Name;
                    readConfirm.UserID = user.ID;
                    readConfirm.Created = DateTime.Now;

                    db.ReadConfirmations.Add(readConfirm);

                }

                db.SaveChanges();
            }
        }
    }
}
