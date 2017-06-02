using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GCPack.Web.Filters;
using GCPack.Model;
using GCPack.Service.Interfaces;

namespace GCPack.Web.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly IDocumentsService documentService;
        public DocumentsController(IDocumentsService documentService)
        {
            this.documentService = documentService;
        }


        // GET: Documents
        [AuthorizeAttributeGC(Roles = "user,admin")]
        public ActionResult Index()
        {
            RizenyDokument dokument = documentService.GetDocument(0);
            return View(dokument);
        }

        [AuthorizeAttributeGC(Roles = "user,admin")]
        public ActionResult Documents()
        {
            RizenyDokument dokument = documentService.GetDocument(0);
            return View(dokument);
        }


        public ActionResult Save(DocumentModel document, IEnumerable<HttpPostedFileBase> upload)
        {
            this.Request.Files[0].SaveAs("");
            return View("Edit");
        }


        public ActionResult getDocs()
        {
            ICollection<DocumentModel> documents = new HashSet<DocumentModel>();
            documents.Add(new DocumentModel() { Title = "title 1" });
            documents.Add(new DocumentModel() { Title = "title 2" });
            documents.Add(new DocumentModel() { Title = "title 3" });
            return Json(documents, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit()
        {
            ViewBag.Type = "Editace dokumentu";
            DocumentModel document = new DocumentModel() {Title = "Dokument 1"};
            document.Users.Add(new UserModel() { LastName = "Navratil" });
            document.Users.Add(new UserModel() { LastName = "Fryc" });
            ViewBag.Documents = document;
            
            return View(document);
        }


    }
}