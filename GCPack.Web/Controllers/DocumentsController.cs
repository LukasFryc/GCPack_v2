using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GCPack.Web.Filters;
using GCPack.Model;
using GCPack.Service.Interfaces;
using System.IO;

namespace GCPack.Web.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly IDocumentsService documentService;
        private readonly IUsersService userService;
        public DocumentsController(IDocumentsService documentService, IUsersService userService)
        {
            this.documentService = documentService;
            this.userService = userService;
        }


        // GET: Documents
        [AuthorizeAttributeGC(Roles = "user,admin")]
        public ActionResult Index()
        {
            DocumentModel dokument = documentService.GetDocument(0);
            return View(dokument);
        }

        [AuthorizeAttributeGC(Roles = "user,admin")]
        public ActionResult Documents()
        {
            DocumentModel dokument = documentService.GetDocument(0);
            return View(dokument);
        }


        public ActionResult Save(DocumentModel document, IEnumerable<HttpPostedFileBase> upload)
        {
            string folderForFiles = System.Configuration.ConfigurationManager.AppSettings["FileTemp"];
            string guid = Guid.NewGuid().ToString();
            ICollection<string> fileNames = new HashSet<string>();
            string folderPath = folderForFiles + guid + @"\";
            Directory.CreateDirectory(folderPath);
            for (int i = 0;i < this.Request.Files.Count; i++)
            {
                if (this.Request.Files[i].ContentLength > 0)
                {
                    string filePath = folderPath + this.Request.Files[i].FileName;
                    fileNames.Add(filePath);
                    this.Request.Files[i].SaveAs(filePath);
                }
            }
            documentService.AddDocument(document,fileNames);
            return RedirectToAction("Edit");
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
            ViewBag.JobPositions = userService.GetJobPositions();
            DocumentModel document = new DocumentModel() {Title = "Dokument 1"};
            ViewBag.Documents = document;
            
            return View(document);
        }


    }
}