using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GCPack.Repository;
using GCPack.Service;
using GCPack.Service.Interfaces;
using GCPack.Model;
using System.Security.Claims;
using GCPack.Web.Filter;

namespace GCPack
{
    public static class Helper
    {
        public static IDocumentsService GetInstance()
        {
            // rucne vytvorene injection objektu aby se nemuselo pouzivat IoC protoze ve staticke tride 
            // se neda vyuzit konstruktor

            return new DocumentsService(
                    new DocumentsRepository(),
                    new MailService(),
                    new UsersService(new UsersRepository(), new LogEventsService(new LogEventsRepository())),
                    new CodeListsService(new CodeListsRepository(), new LogEventsService(new LogEventsRepository())),
                    new LogEventsService(new LogEventsRepository())
                    );
        }



        // v pripade ze uzivatel nema opravneni tak se prida 
        // css class
        public static string IsAccessCSSClass(string forRole, string type)
        {
            switch (type)
            {
                case "noDisabled":
                    if (UserRoles.IsAccess(forRole))
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return $@" clsDisabled ";
                    }
                    break;
            }
            return "";
        }



        // nacteni konkretniho dokumentu
        public static DocumentModel GetDocument(int documentId, int forUserId)
        {
            ClaimsPrincipal principal = (ClaimsPrincipal)HttpContext.Current.User;
            return GetInstance().GetDocuments(new DocumentFilter() { DocumentID = documentId, ForUserID = forUserId }).Documents.FirstOrDefault();
        }

        public static ICollection<Item> GetDocumentTypeItems()
        {
            return GetInstance().GetDocumentTypes();
        }

        

    }
}