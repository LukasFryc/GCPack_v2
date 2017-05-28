using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;

namespace GCPack.Web.Filter
{
    public static class UserRoles
    {
        private static string GetRoles()
        {
            ClaimsPrincipal principal = (ClaimsPrincipal)HttpContext.Current.User;
            return System.Convert.ToString(principal.Claims.SingleOrDefault(c => c.Type == "Role").Value);

        }

        public static bool HasRole(string roles)
        {
            bool hasRole = false;
            roles = "," + roles + ",";
            foreach (string userRole in GetRoles().Split(','))
            {
                if (roles.IndexOf("," + userRole + ",") > -1) hasRole = true;
            }

            return hasRole;
        }


        public static string[] GetUserRoles()
        {
            return GetRoles().Split(',');
        }
    }
}