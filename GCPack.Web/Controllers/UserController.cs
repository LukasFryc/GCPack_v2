﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GCPack.Service;
using GCPack.Model;
using GCPack.Service.Interfaces;

namespace GCPack.Web.Controllers
{
    public class UserController : Controller
    {
        readonly IUsersService userService;
        public UserController(IUsersService userService)
        {
            this.userService = userService;
        }

        public ActionResult Index()
        {
            return View(userService.GetUsers(new UserFilter ()));
        }

        public ActionResult ShowAddUserDialog()
        {
            return View("Edit");
        }

        public ActionResult GetUsers(string name, int jobPositionId, string preservedUsers)
        {
            jobPositionId = (jobPositionId == null) ? 0 : jobPositionId;
            int[] excludedUsersId = (!string.IsNullOrEmpty(preservedUsers)) ? Array.ConvertAll(preservedUsers.Split(','), int.Parse) : new int[] { };

            ICollection<int> jobPosition = new HashSet<int>();
            jobPosition.Add(jobPositionId);

            var users = userService.GetUsers(new UserFilter() {
                Name = name,
                ExcludedUsersId = excludedUsersId,
                JobPositionIDs = jobPosition
            });

            
            return Json(users,JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveUser(UserModel user)
        {
            userService.SaveUser(user);
            return RedirectToAction("Index");
        }

        public ActionResult AddUser()
        {
            UserModel user = new UserModel() {JobPositions = new HashSet<int>(), JobPositionIDs = new HashSet<int>() };
            ViewBag.Title = "Přidat osobu";
            ViewBag.Type = "Add";
            ViewBag.JobPositions = userService.GetJobPositions();
            ViewBag.Roles = userService.GetRoles();
            ViewBag.Users = userService.GetUserList(new UserFilter());
            return View("Edit",user);
        }

        public ActionResult EditUser(int userId)
        {
            ViewBag.Title = "Upravit osobu";
            ViewBag.Type = "Edit";
            ViewBag.Users = userService.GetUserList(new UserFilter());
            ViewBag.JobPositions = userService.GetJobPositions();
            ViewBag.Roles = userService.GetRoles();
            UserModel user = userService.GetUser(userId);
            return View("Edit",user);
        }

        public ActionResult DeleteUser(int userId)
        {
            userService.DeleteUser(userId);
            return RedirectToAction("Index");
        }

    }
}