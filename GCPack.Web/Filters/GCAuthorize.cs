using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Claims;
using GCPack.Model;
using GCPack.Web.Filter;


namespace GCPack.Web.Filters
{
    public class GCAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase filterContext)
        {


            return UserRoles.IsAccess(this.Roles);
        }
    }

}