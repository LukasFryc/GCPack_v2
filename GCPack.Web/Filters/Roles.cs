using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;
using GCPack.Model;

    namespace GCPack.Web.Filter
{
    public static class UserRoles
    {

        private static string GetRoles()
        {
            
            ClaimsPrincipal principal = (ClaimsPrincipal)HttpContext.Current.User;
            if (principal.Claims.Count() > 0)
            {
                return System.Convert.ToString(principal.Claims.SingleOrDefault(c => c.Type == "Role").Value);
            }
            else
            {
                return string.Empty;
            }

        }


        public static bool IsAccess(string forRoles)
        {

            string urcenoProRole = "," + forRoles + ",";

            ClaimsPrincipal principal = (ClaimsPrincipal)HttpContext.Current.User;
            string Roles = "";
            if (principal.Claims.SingleOrDefault(c => c.Type == "Role") != null)
            {
                Roles = "," + System.Convert.ToString(principal.Claims.SingleOrDefault(c => c.Type == "Role").Value) + ",";
            }
            
            string documentID = HttpContext.Current.Request["ID"];

            if (!string.IsNullOrEmpty(documentID)) documentID = documentID.Replace(",", "");

            int UserId = -1;
            string UserName = "";
            if (principal.Claims.SingleOrDefault(c => c.Type == "UserId") != null)
            {
                UserId = System.Convert.ToInt32(principal.Claims.SingleOrDefault(c => c.Type == "UserId").Value);

                UserName = principal.Claims.SingleOrDefault(c => c.Type == "UserName").Value;

            }
            

            // autorizace
            bool access = false;
            foreach (string role in urcenoProRole.ToLower().Split(','))
            {
                if (Roles.ToLower().IndexOf("," + role + ",") > -1 && Roles != ",,") access = true;

                switch (role)
                {
                    case "documentauthorid":
                        DocumentModel dm3 = Helper.GetDocument(System.Convert.ToInt32(documentID), UserId);
                        access = (access == true) ? true : (dm3.AuthorID == UserId);
                        break;
                    case "documentadminid":
                        DocumentModel dm2 = Helper.GetDocument(System.Convert.ToInt32(documentID), UserId);
                        // AdministratorID
                        // access = (access == true) ? true : (dm2.DocumentAdministrator.Contains(UserName));
                        access = (access == true) ? true : (dm2.AdministratorID == UserId);
                        break;
                }
            }
            return access;
        }



        public static string UserName()
        {
            ClaimsPrincipal principal = (ClaimsPrincipal)HttpContext.Current.User;
            if (principal.Claims.Count() > 0)
            {
                return System.Convert.ToString(principal.Claims.Where(p => p.Type.ToLower() == "username").SingleOrDefault().Value);
            }
            else
            {
                return "";
            }

        }


        public static int GetUserId()
        {
            ClaimsPrincipal principal = (ClaimsPrincipal)HttpContext.Current.User;
            if (principal.Claims.Count() > 0)
            {
                return System.Convert.ToInt32(principal.Claims.Where(p => p.Type.ToLower() == "userid").SingleOrDefault().Value);
            }
            else
            {
                return 0;
            }
            
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