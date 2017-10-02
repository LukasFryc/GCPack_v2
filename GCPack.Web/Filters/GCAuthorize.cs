using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Claims;
using GCPack.Model;

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
            string documentID = filterContext.Request["DocumentID"];
            int UserId = System.Convert.ToInt32(principal.Claims.SingleOrDefault(c => c.Type == "UserId").Value);

            // autorizace
            bool access = false;
            foreach (string role in urcenoProRole.ToLower().Split(','))
            {
                if (Roles.ToLower().IndexOf("," + role + ",") > -1) access = true;

                switch (role)
                {
                    case "documentowner":
                        DocumentModel dm1 = Helper.GetDocument(System.Convert.ToInt32(documentID), UserId);
                        access = (access == true) ? true : (dm1.OwnerID == UserId);
                        break;
                    case "documentadmin":
                        DocumentModel dm2 = Helper.GetDocument(System.Convert.ToInt32(documentID), UserId);
                        access = (access == true) ? true : (dm2.AdministratorID == UserId);
                        break;
                }
            }
            return access;
        }
    }

}