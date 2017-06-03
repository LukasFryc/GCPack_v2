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

        public ActionResult GetUsers(string name, int jobPositionId, Array[] preservedUsers)
        {

            var users = userService.GetUsers(new UserFilter() {Name = name, JobPositionID = jobPositionId });

            
            return Json(users,JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddUser(UserModel user)
        {
            userService.AddUser(user);
            return RedirectToAction("Index");
        }

        public ActionResult DeleteUser(int userId)
        {
            userService.DeleteUser(userId);
            return RedirectToAction("Index");
        }

    }
}