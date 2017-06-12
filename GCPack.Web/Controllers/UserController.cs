using System;
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

        public ActionResult GetUsers(string name, int? jobPositionId, string preservedUsers)
        {
            jobPositionId = (jobPositionId == null) ? 0 : jobPositionId;
            int[] excludedUsersId = (!string.IsNullOrEmpty(preservedUsers)) ? Array.ConvertAll(preservedUsers.Split(','), int.Parse) : new int[] { };
            
            var users = userService.GetUsers(new UserFilter() {
                Name = name,
                JobPositionID = (int) jobPositionId,
                ExcludedUsersId = excludedUsersId
            });

            
            return Json(users,JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveUser(UserModel user)
        {
            return RedirectToAction("Index");
        }

        public ActionResult AddUser()
        {
            UserModel user = new UserModel();
            ViewBag.Title = "Přidat osobu";
            ViewBag.Type = "Add";
            return View("Edit",user);
        }

        public ActionResult EditUser(int userId)
        {
            ViewBag.Title = "Upravit osobu";
            ViewBag.Type = "Edit";
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