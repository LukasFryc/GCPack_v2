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
        [AuthorizeAttributeGC(Roles = "user,admin,supervisor")]
        public ActionResult Index(string Message)
        {
            // TODO: dopsat filtrovani - pridat do filtru userId pro ktereho se vyberou pouze jeho dokumenty
            ICollection<DocumentModel> documents = documentService.GetDocuments(new DocumentFilter());
            ViewBag.Message = Message;
            return View(documents);
        }
        

        public ActionResult Save(DocumentModel document, IEnumerable<HttpPostedFileBase> upload, string type)
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
            switch (type)
            {
                case "Add":
                    documentService.AddDocument(document, fileNames);
                    break;
                case "Edit":
                    documentService.EditDocument(document, fileNames);
                    break;
            }
            
            return RedirectToAction("Index", new { Message = "Dokument byl uložen." });
        }

        public ActionResult Add()
        {
            ViewBag.Type = "Nový řízený dokument";
            ViewBag.JobPositions = userService.GetJobPositions();
            DocumentModel document = new DocumentModel() {Title = ""};
            ViewBag.Documents = document;
            ViewBag.Type = "Add";
            return View("edit",document);
        }

        public ActionResult Edit(int documentId)
        {
            ViewBag.Type = "Úprava řízeného dokumentu";
            ViewBag.JobPositions = userService.GetJobPositions();
            DocumentModel document = documentService.GetDocument(documentId);
            ViewBag.Documents = document;
            ViewBag.Type = "Edit";
            return View("edit", document);
        }




    }
}