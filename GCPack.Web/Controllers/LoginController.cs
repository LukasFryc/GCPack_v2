using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GCPack.Model;
using System.Web.Security;
using GCPack.Service.Interfaces;

namespace GCPack.Web.Controllers
{
    public class LoginController : Controller
    {
        readonly IUsersService usersService;
        public LoginController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        // GET: Login
        public ActionResult Index(GCPack.Model.UserModel userModel)
        {
            return View(userModel);
        }

        
        public ActionResult LoggedOut(string message)
        {
            return View((object)message);
        }

        public ActionResult Logout()
        {
            Response.Cookies[FormsAuthentication.FormsCookieName].Expires = DateTime.Now.AddDays(-1);
            return View("LoggedOut");
        }

        public ActionResult Login(UserModel model)
        {

            string token = usersService.Login(model.UserName, model.Password);

            if (token != null)
            {
                
                // TODO - servisni vrstva overi login a pwd a vygeneruje se token a prida 
                // se zaznam do databaze do tabulky logins
                // vygeneruje se formsauthenticationticket do ktereho se ulozi do data ticket

                // tento ticket se vzdy bude overovat zda existuje v databazi v autentizacnim attributu

                var ticket = new FormsAuthenticationTicket(1, //version
                        model.UserName, // user name
                        DateTime.Now,             //creation
                        DateTime.Now.AddMinutes(30), //Expiration
                        false, //Persistent
                        token
                        );

                string encTicket = FormsAuthentication.Encrypt(ticket);

                // Create the cookie.
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                return this.RedirectToAction("Index", "Home");
            }


            return this.RedirectToAction("LoggedOut", "Login", new { Message = "Spatny login nebo heslo." } );

        }

    }
}