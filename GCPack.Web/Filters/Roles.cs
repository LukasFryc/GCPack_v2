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


            // LF 30.10.2017 upravena z duvodu rozdilneho volani url z indexu (ve url se pouziva documentid)
            // yatimco na jiz otevrenem dokument k edirtaci exituje pouze  field ID,coz se pouziva napr pri ukladani 
            // i pri napr autorizaci v autorizacnim atributu 
            string documentID = HttpContext.Current.Request["ID"];
            if (string.IsNullOrEmpty(documentID)) documentID = HttpContext.Current.Request["documentid"];


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
                        // LF 30.10.2017 osetrni pripadu kdz dokument je novy a nema ID
                        if (documentID != "0" && documentID != null)
                        {
                            // Getdocument totiz vraci firstordefault tj pokud mu dame hledat dokument s ID 0 pak vracel dokument napr s id 18
                            
                            DocumentModel dm3 = Helper.GetDocument(System.Convert.ToInt32(documentID), UserId);
                            
                            access = (access == true) ? true : (dm3.AuthorID == UserId);
                        } else
                        {
                            access = true;
                        }
                        break;
                    case "documentadminid":
                        if (documentID != "0" && documentID != null)
                        {
                            
                            DocumentModel dm2 = Helper.GetDocument(System.Convert.ToInt32(documentID), UserId);
                            
                            access = (access == true) ? true : (dm2.AdministratorID == UserId);
                            // AdministratorID
                            // access = (access == true) ? true : (dm2.DocumentAdministrator.Contains(UserName));
                        }
                        else
                        {
                            access = true;
                        }
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