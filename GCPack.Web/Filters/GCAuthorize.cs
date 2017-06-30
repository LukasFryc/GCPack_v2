using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Claims;

namespace GCPack.Web.Filters
{
    public class AuthorizeAttributeGC : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase filterContext)
        {
            // nactou se z atributu vsechny role pro ktere je tato akce urcena
            string urcenoProRole = "," + this.Roles + ",";
            
            ClaimsPrincipal principal = (ClaimsPrincipal)HttpContext.Current.User;
            string Roles = System.Convert.ToString(principal.Claims.SingleOrDefault(c => c.Type == "Role").Value);

            // autorizace
            bool access = false;
            foreach (string role in Roles.ToLower().Split(','))
            {
                if (urcenoProRole.ToLower().IndexOf("," + role + ",") > -1) access = true;
            }
            return access;
        }
    }

}