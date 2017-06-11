using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GCPack.Web.Filters;
using GCPack.Model;
using GCPack.Service.Interfaces;
using System.IO;
using GCPack.Web.Filter;

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
        [AuthorizeAttributeGC(Roles = "user,admin,supervisor,poweruser")]
        public ActionResult Index(string Message)
        {
            // TODO: dopsat filtrovani - pridat do filtru userId pro ktereho se vyberou pouze jeho dokumenty
            ICollection<DocumentModel> documents = new HashSet<DocumentModel>();
            
            ICollection<Item> documentTypes = documentService.GetDocumentTypes();
            documentTypes.Add(new Item { ID = 0, OrderBy = 0, Value = "Všechny dokumenty" });
            ViewBag.DocumentTypes = documentTypes.OrderBy(dt => dt.OrderBy);
            ViewBag.Message = Message;
            return View(documents);
        }

        public ActionResult GetFile(int fileID)
        {
            FileItem fileItem = documentService.GetFile(fileID);
            Response.AppendHeader("Content-Disposition", "attachment; filename = " + fileItem.Name);
            return File(fileItem.Data, "attachment");
        }

        public ActionResult Details(int documentId)
        {
            int userId = UserRoles.GetUserId();
            return View(documentService.GetDocument(documentId, userId));
        }


        public ActionResult GetDocuments(DocumentFilter filter)
        {
            filter.ForUserID = UserRoles.GetUserId();
            ICollection<DocumentModel> documents = documentService.GetDocuments(filter);
            return Json(documents, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Readed(int documentID)
        {
            // seznameni s dokumentem
            int userId = UserRoles.GetUserId();

            documentService.Readed(documentID, userId);
            return View("Details",documentService.GetDocument(documentID, userId));
            
            
        }

        public ActionResult Save(DocumentModel document, IEnumerable<HttpPostedFileBase> upload, string type)
        {
            string folderForFiles = System.Configuration.ConfigurationManager.AppSettings["FileTemp"];
            string guid = Guid.NewGuid().ToString();
            ICollection<string> fileNames = new HashSet<string>();
            string folderPath = folderForFiles + guid + @"\";
            Directory.CreateDirectory(folderPath);

            bool x = ModelState.IsValid;

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
            ViewBag.DocumentTypes = documentService.GetDocumentTypes();
            ViewBag.Type = "Nový řízený dokument";
            ViewBag.JobPositions = userService.GetJobPositions();
            DocumentModel document = new DocumentModel() { Revision = "P"};
            ViewBag.Documents = document;
            ViewBag.Administrators = userService.GetUsers(new UserFilter() { });
            ViewBag.Type = "Add";
            return View("edit",document);
        }

        public ActionResult Delete(int documentId)
        {
            documentService.DeleteDocument(documentId);
            return RedirectToAction("Index", new { Message = "Dokument byl smazán." });
        }

        public ActionResult Edit(int documentId)
        {

            ViewBag.DocumentTypes = documentService.GetDocumentTypes();
            int userId = UserRoles.GetUserId();
            ViewBag.Type = "Úprava řízeného dokumentu";
            ViewBag.JobPositions = userService.GetJobPositions();
            DocumentModel document = documentService.GetDocument(documentId, userId);
            ViewBag.Documents = document;
            ViewBag.Administrators = userService.GetUsers(new UserFilter() { });
            ViewBag.Type = "Edit";
            return View("edit", document);
        }




    }
}