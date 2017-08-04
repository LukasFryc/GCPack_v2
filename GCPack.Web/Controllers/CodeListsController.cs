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
    public class CodeListsController : Controller
    {
        readonly IDocumentsService documentsService;
        readonly IUsersService usersService;

        public CodeListsController(IDocumentsService documentsService, IUsersService usersService)
        {
            this.documentsService = documentsService;
            this.usersService = usersService;
        }
        public ActionResult DocumentTypesIndex()
        {
            ICollection<Item> documentTypes = documentsService.GetDocumentTypes();
            return View(documentTypes);
        }

        public ActionResult DocumentTypesEdit(int ID)
        {
            ViewBag.Title = "Editace typu dokumentu";
            ICollection<Item> Users = new HashSet<Item>();

            Users = usersService.GetUserList(new UserFilter());
            Users.Add(new Item() { ID = 0, Value = "---------------", OrderBy = 0 });
            Users = Users.OrderBy(u => u.ID).ToList();
            ViewBag.Users = Users;
            DocumentTypeModel documentType = documentsService.GetDocumentType(ID);
            return View(documentType);
        }

        public ActionResult DocumentTypesAdd()
        {
            ViewBag.Title = "Nový typu dokumentu";


            ICollection<Item> Users = new HashSet<Item>();
            
            Users = usersService.GetUserList(new UserFilter());
            Users.Add(new Item() { ID = 0, Value = "---------------", OrderBy = 0 });
            Users = Users.OrderBy(u => u.ID).ToList();
            ViewBag.Users = Users;
            return View("DocumentTypesEdit",new DocumentTypeModel());
        }

        public ActionResult SaveDocumentType(DocumentTypeModel documentType)
        {
            documentsService.SaveDocumentType(documentType);
            return RedirectToAction("DocumentTypesIndex");
        }
        


    }
}
