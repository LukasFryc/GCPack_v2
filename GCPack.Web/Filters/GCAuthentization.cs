﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Security.Principal;
using System.Web.Security;
using System.Security.Claims;
using GCPack.Service.Interfaces;
using GCPack.Model;

namespace GCPack.Web.Filters
{

    interface MyIPrincipal : IPrincipal
    {
        string Id { get; set; }
        string UserName { get; set; }
    }

    public class GCAuthentization : FilterAttribute, IAuthenticationFilter
    {

        readonly IUsersService userService;
        public GCAuthentization(IUsersService userService)
        {
            this.userService = userService;
        }

        public void OnAuthentication(AuthenticationContext context)
        {

            // pokud se pristupuje z login controlleru pak se neoveruje autentizace
            // protoze uzivatel jeste neni prihlaseny

            if (context.ActionDescriptor.ControllerDescriptor.ControllerName == "Login") return; 
            

            // pokud existuje autentizacni cookie, tak ji pouziji
            var authCookie = context.HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                string encTicket = authCookie.Value;
                if (!String.IsNullOrEmpty(encTicket))
                {

                    FormsAuthenticationTicket aCookie = null;
                    try
                    {
                        aCookie = FormsAuthentication.Decrypt(encTicket);
                    }
                    catch (Exception e)
                    {
                        FormsAuthentication.SignOut();
                    }
                    UserModel user = null;

                    if (aCookie != null)
                    {
                        user = userService.GetUser(aCookie.UserData);
                    }
                    
                    
                    if (user != null)
                    {
                        IList<Claim> listOfClaims = new List<Claim>() {
                        new Claim(ClaimTypes.Name,user.UserName),
                        new Claim("Role", user.Roles),
                        new Claim("FirstName", user.FirstName),
                        new Claim("UserName", user.LastName + " " + user.FirstName),
                        new Claim("UserId", user.ID.ToString())};
                        ClaimsIdentity identita = new ClaimsIdentity(listOfClaims, "User identity");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identita);
                        if (HttpContext.Current != null) HttpContext.Current.User = claimsPrincipal;
                    }
                }
            }



            // pokud neni uzivatel autentizovany, tak ho poslu autentizovat
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                
                context.Result = new HttpUnauthorizedResult(); // mark unauthorized
            }
        }
        public void OnAuthenticationChallenge(AuthenticationChallengeContext context)
        {
            if (context.Result == null || context.Result is HttpUnauthorizedResult)
            {
                context.Result = new RedirectToRouteResult("Default",
                new System.Web.Routing.RouteValueDictionary{
                {"controller", "Login"},
                {"action", "Login"},
                { "rawUrl", context.HttpContext.Request.RequestContext.HttpContext.Request.RawUrl }
                });
            }
        }
    }
    
}