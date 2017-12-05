using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GCPack.Web.Filter;
using GCPack.Web.Filters;
using GCPack.Model;
using GCPack.Service.Interfaces;
using System.IO;
using System.Data.Entity.Core.Objects;
using System.Web.UI;

namespace GCPack.Web.Controllers
{
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class DocumentsController : Controller
    {
        private readonly IDocumentsService documentService;
        private readonly IUsersService userService;
        private readonly ICodeListsService codeListService;

        public DocumentsController(IDocumentsService documentService, IUsersService userService, ICodeListsService codeListService)
        {
            this.documentService = documentService;
            this.userService = userService;
            this.codeListService = codeListService;
        }


        [GCAuthorizeAttribute(Roles = "SystemAdmin,SuperDocAdmin,DocAdmin,User,Author")]
        public ActionResult Index(DocumentFilter filter)
        {
            
            // TODO: dopsat filtrovani - pridat do filtru userId pro ktereho se vyberou pouze jeho dokumenty
            ICollection<DocumentModel> documents = new HashSet<DocumentModel>();
            ICollection<Item> documentTypes = documentService.GetDocumentTypes();

            Session["documentFilter"] = filter;
            if (filter.Reset) Session["documentFilter"] = null;

            //if (filter.DocumentTypeID != null && filter.DocumentTypeID != 0) {

                // TODO: Zvazit upravu cislenikovych trid o dedeni z tridy item
                // TODO: Napreklad pri volani funkce documentService.GetDocumentTypes se varci IColection<Item>
                // TODO: zatimco documentService.GetDocumentType vraci DocumentTypeModel
                // TODO: zvazit zda ciselnikove tridy by nemel dedti z obecne tridy Item, je zde samozrejme problem 
                // TODO: s jiz stavajicim pouzitim trid a s rozdily v propertach (Value x Name a orderBy x OrderBy) 
                // TODO: mozna se to budoucnu toto precovani muze hodit - mohl bych pretypovat DocumentTypeModel nebo ProjectModel nebo DivisionMOdel na Item


                // int v2 = 0;
                // if (DocumentTypeID.HasValue) v2 = DocumentTypeID.Value;
                //DocumentTypeModel  choiceDocumentType = documentService.GetDocumentType(v2);

               //Item choiceDocumentType = documentTypes.Where(dc => dc.ID ==filter.DocumentTypeID).Select(dc => dc).FirstOrDefault();
               //choiceDocumentType.OrderBy = -1;
            //}

            //ICollection<Item> documentSortType = new HashSet<Item>(); ;
            //documentSortType.Add(new Item { ID = 0, Code = "",  OrderBy = 0, Value = "Sestupně" });

            //ICollection<Item> documentSort = new HashSet<Item>(); ;
            //documentSort.Add(new Item { ID = 0, Code = "", OrderBy = 0, Value = "Setřídit podle" });
            //documentSort.Add(new Item { ID = 1, Code = "Title", OrderBy = 0, Value = "Názvu" });
            //documentSort.Add(new Item { ID = 2, Code = "DocumentNumber", OrderBy = 0, Value = "Čísla" });
            //documentSort.Add(new Item { ID = 3, Code = "DocumentAdminType", OrderBy = 0, Value = "Správce" });
            //documentSort.Add(new Item { ID = 4, Code = "EffeciencyDate", OrderBy = 0, Value = "Data účinnosti" });
            //ViewBag.DocumentSort = documentSort.OrderBy(dt => dt.OrderBy);


            ICollection<Item> ReviewNecessaryChange = new HashSet<Item>();
            ReviewNecessaryChange.Add(new Item { ID = 0, Code = "all", OrderBy = 1, Value = "Všechny" });
            ReviewNecessaryChange.Add(new Item { ID = 1, Code = "necessaryChange", OrderBy = 2, Value = "Nutná změna" });
            ViewBag.ReviewNecessaryChange = ReviewNecessaryChange.OrderBy(rt => rt.OrderBy);


            //ICollection<UserModel> users = new HashSet<UserModel>();
            //users = userService.GetUsers(new UserFilter());
            //users.Add(new UserModel() { ID = 0, Name = "Zvolte osobu", orderBy = 1 });
            //ViewBag.Projects = users.OrderByDescending(p => p.orderBy).ThenBy(p => p.Name);

            ICollection<Item> revisionType = new HashSet<Item>(); ;
            revisionType.Add(new Item { ID = 0, Code = "all", OrderBy = 0, Value = "Všechny" });
            revisionType.Add(new Item { ID = 1, Code = "p", OrderBy = 1, Value = "Platné (P + R)" });
            revisionType.Add(new Item { ID = 2, Code = "n", OrderBy = 2, Value = "Neplatné (N)" });
            revisionType.Add(new Item { ID = 3, Code = "r", OrderBy = 3, Value = "V revizi (R)" });
            ViewBag.RevisionType = revisionType.OrderBy(rt => rt.OrderBy);

            ICollection<Item> readType = new HashSet<Item>(); ;
            readType.Add(new Item { ID = 0, Code = "all", OrderBy = 0, Value = "Všechny" });
            readType.Add(new Item { ID = 2, Code = "read", OrderBy = 1, Value = "Seznámeno" });
            readType.Add(new Item { ID = 1, Code = "unread", OrderBy = 2, Value = "Neseznámeno" });
            ViewBag.ReadType = readType.OrderBy(dt => dt.OrderBy);

            documentTypes.Add(new Item { ID = 0, OrderBy = -1, Value = "Všechny typy" });
            ViewBag.DocumentTypes = documentTypes.OrderBy(dt => dt.OrderBy);

            ICollection<DocumentStateModel> documentStates = new HashSet<DocumentStateModel>();
            documentStates = codeListService.GetDocumentStates();
            //DocumentStateModel documentState = new DocumentStateModel();
            //documentState = documentStates.Where(p => p.ID = ((filter != null && filter.StateID != null) ? filter.StateID : string.Empty)).Select(p => p).FirstOrDefault(); 

            documentStates.Add(new DocumentStateModel() { ID = 0, Code = "all", Name = "Všechny stavy", orderBy = -1 });
            ViewBag.DocumentStates = documentStates.OrderBy(p => p.orderBy);

            ICollection<ProjectModel> projects = new HashSet<ProjectModel>();
            projects = codeListService.GetProjects();
            projects.Add(new ProjectModel() { ID = 0, Name = "Všechny projekty", orderBy = 1 });
            ViewBag.Projects = projects.OrderByDescending(p => p.orderBy).ThenBy(p => p.Name);

            ICollection<DivisionModel> divisions = new HashSet<DivisionModel>();
            divisions = codeListService.GetDivisions();
            divisions.Add(new DivisionModel() { ID = 0, Name = "Všechna střediska", orderBy = 1 });
            ViewBag.Divisions = divisions.OrderByDescending(p => p.orderBy).ThenBy(p => p.Name);

            ICollection<AppSystemModel> appSystems = new HashSet<AppSystemModel>();
            appSystems = codeListService.GetAppSystems();
            appSystems.Add(new AppSystemModel() { ID = 0, Name = "Všechny systémy", orderBy = 1 });
            ViewBag.AppSystems = appSystems.OrderByDescending(p => p.orderBy).ThenBy(p => p.Name);

            ICollection<WorkplaceModel> workplaces = new HashSet<WorkplaceModel>();
            workplaces = codeListService.GetWorkplaces();
            workplaces.Add(new WorkplaceModel() { ID = 0, Name = "Všechna pracoviště", orderBy = 1 });
            ViewBag.Workplaces = workplaces.OrderByDescending(p => p.orderBy).ThenBy(p => p.Name);

            //@DivisionID int,
            //@AppSystemID int,
            //@WorkplaceID int

            // ViewBag.Message = Message;

            

            return View(filter);
            //DocumentFilter filterModel = new DocumentFilter() { WorkplaceID = 1, AppSystemID = 1, StateID = 2 };
            //return View(filterModel);

        }

        private void SendMail()
        {
            string x = System.Configuration.ConfigurationManager.AppSettings[""];

            /* 
              
             email se odesila na tyto akce:
             1.) evidence rizeneho dokumentu - odesle se email vsem kteri se maji s dokumentem seznamit
                - emailova adresa se pouzije: user.email1
             2.) posilani posty spravci ze ma dokument prezkoumat - nekolik dni (v configu) pred terminem pristiho prezkoumani nextRevision
             3.) posilani emailu spravci kdyz se bude blizit (v configu pocet dni) termin platnosti EndDate - do kdy to plati
                 datum ucinnosti je EffeciencyDate - od kdy to plati
             4.) kdyz se prida osobe funkce, pak se prida do rozdelovniku vsech dokumentu kde je uvedena tato funkce 
                a prijde mu seznam vsech dokumentu s linkama ktere ma precist (seznamit se)
                - zaroven se zapise datum kdy byl uzivatel pridan do rozdelovniku
                
             5.) v tabulce typ rizeneho dokumentu se doplni novy sloupec pocet dni do kdy se ma seznamit uzivatel s dokumentem
                - pokud se neseznami do datumu seznameni se a zaroven datum seznameni se je mensi nez datum kdy byl uzivatel pridan 
                do rozdelovniku, pak mu prijde po tomto terminu vzdy kazdy den a posle se email ve kterem bude seznam vsech


             6.) uzivatel muze mit vice pracovnich pozic - nova tabulka JobUserMap - v editaci uzivatele pak moznost zaradit uzivatele do
                vice pracovnich pozic

             7.) vytvori se novy rozdelovnik pro funkce - rozsirit 

             */

            documentService.SendEmail();
        }

        [GCAuthorize(Roles = "SystemAdmin,SuperDocAdmin,DocAdmin,Author,User")]
        public ActionResult GetFile(int fileID, bool forceDownload)
        {
            // LF do 9.11.2017 takto to udelal dave
            // puvodni action nemela parametr forceAttachment
            //FileItem fileItem = documentService.GetFile(fileID);
            //Response.AppendHeader("Content-Disposition", "attachment; filename = " + fileItem.Name);
            //return File(fileItem.Data, "attachment");

            // LF 9.11.2017 
            FileItem fileItem = documentService.GetFile(fileID);
            string name = Path.GetFileName(fileItem.Name);
            string ext = Path.GetExtension(fileItem.Name);
            string type = "";
            // set known types based on file extension  
            if (ext != null)
            {
                switch (ext.ToLower())
                {
                    case ".htm":
                    case ".html":
                        type = "text/HTML";
                        break;
                    case ".txt":
                        type = "text/plain";
                        break;
                    case ".doc":
                        type = "Application/msword";
                        break;
                    case ".xls":
                        type = "application/excel";
                        break;
                    case ".rtf":
                        type = "text/richtext";
                        break;
                    case ".pdf":
                        type = "application/pdf";
                        break;
                }
            }

            // LF pokud nenajdu pripomu v mime case tak  neumoznim uzivateli dokument zobarazit ale jen stahnout downloadedm
            if (type == "") {
                type = "attachment";
                forceDownload = true;
            };

            if (forceDownload)
            {
                Response.AppendHeader("content-disposition", "attachment; filename=" + fileItem.Name);
                type = "attachment";
            }
            else
            {
                Response.AppendHeader("Content-Disposition", "inline; filename=" + fileItem.Name);
            }


            return File(fileItem.Data, type);

            // https://www.sitepoint.com/mime-types-complete-list/

            // TODO: LF 9.11.2017  nacitani z configu trochu o dost lepe 
            // https://stackoverflow.com/questions/21878374/how-to-read-multiple-values-in-c-sharp-app-config-file
        }

        private void DownloadFile(string fname, bool forceDownload)
        {
            string path = System.Web.HttpContext.Current.Server.MapPath(fname); 
            string name = Path.GetFileName(path);
            string ext = Path.GetExtension(path);
            string type = "";
            // set known types based on file extension  
            if (ext != null)
            {
                switch (ext.ToLower())
                {
                    case ".htm":
                    case ".html":
                        type = "text/HTML";
                        break;

                    case ".txt":
                        type = "text/plain";
                        break;

                    case ".doc":
                    case ".rtf":
                        type = "Application/msword";
                        break;
                }
            }
            if (forceDownload)
            {
                Response.AppendHeader("content-disposition",
                    "attachment; filename=" + name);
            }
            if (type != "")
                Response.ContentType = type;
            Response.WriteFile(path);
            Response.End();
        }


        [GCAuthorize(Roles = "SystemAdmin,SuperDocAdmin,DocAdmin,Author,User")]
        public ActionResult Details(int documentId, string tabs="")
        {
            //int userId = UserRoles.GetUserId();
            //return View(documentService.GetDocument(documentId, userId));

            int userId = UserRoles.GetUserId();
            DocumentModel document = documentService.GetDocument(documentId, userId);
            DocumentTypeModel typeModel = documentService.GetDocumentType(document.DocumentTypeID);
            ViewBag.TypeModel = typeModel;
            ViewBag.Documents = document;
            ViewBag.Administrators = userService.GetUserList(new UserFilter() { });
            ViewBag.Type = "Detail";
            ViewBag.Tabs = tabs;
            InitCodeLists();
            return View("Details", document);

        }

        [GCAuthorize(Roles = "SystemAdmin,SuperDocAdmin,documentauthorid,documentadminid")]
        public ActionResult Registered(DocumentModel document)
        {
            if (ModelState.IsValid)
            { 
                documentService.ChangeDocumentState(document.ID, "Registered");
                return View();
            }
            //return View("Edit",document);
            if (document.ID == 0)
            {
                return Add(document.DocumentTypeID);
            }
            else
            {
                return Edit(document.ID);
            }
        }


        [GCAuthorize(Roles = "SystemAdmin,SuperDocAdmin")]
        public ActionResult GetDocumentsForUser(DocumentFilter filter)
        {
            Session["documentFilter"] = filter;
            ICollection<DocumentModel> documents = documentService.GetDocuments(filter).Documents;
            return Json(documents, JsonRequestBehavior.AllowGet);
        }

        [GCAuthorize(Roles = "SystemAdmin,SuperDocAdmin,DocAdmin,Author,User")]
        public ActionResult GetDocuments(DocumentFilter filter)
        {

            // filter.EffeciencyDateFrom = string.IsNullOrEmpty(filter.EffeciencyDateFrom.ToString())
            //? (DateTime?)null
            //: DateTime.Parse(filter.EffeciencyDateFrom.ToString());
            filter.ForUserID = UserRoles.GetUserId();
            if (filter.SaveFilter)
            {
                Session["documentFilter"] = filter;
            }

            //ICollection<DocumentModel> documents = documentService.GetDocuments(filter);
            DocumentCollectionModel documentCollection = documentService.GetDocuments(filter);
            
            return Json(documentCollection, JsonRequestBehavior.AllowGet);
        }

        // seznameni s dokumentem
        [GCAuthorize(Roles = "SystemAdmin,SuperDocAdmin,DocAdmin,Author,User")]
        public ActionResult Readed(int ID)
        {
            int userId = UserRoles.GetUserId();
            documentService.Readed(ID, userId);
            DocumentFilter filter = (DocumentFilter)Session["DocumentFilter"];
            return RedirectToAction("Index", filter);
        }

        [GCAuthorize(Roles = "SystemAdmin,SuperDocAdmin,documentadminid,documentauthorid")]
        public ActionResult RegisterDocument(DocumentModel document, IEnumerable<HttpPostedFileBase> upload, string type, string HelpText)
        {
            if (ModelState.IsValid)
            {
                DocumentFilter filter = (DocumentFilter)Session["DocumentFilter"];
                ICollection<string> fileNames = SaveFiles();
                int userId = UserRoles.GetUserId();

                if (type == "Add")
                {

                    document.IssueNumber = 1;
                    documentService.AddDocument(document, fileNames, userId);
                    fileNames = null;
                }
                documentService.RegisterDocument(document, fileNames, userId);
                return RedirectToAction("Index", filter);
            }

            //return View("Edit",document);
            if (document.ID == 0)
            {
                return Add(document.DocumentTypeID);
            }
            else
            {
                return Edit(document.ID);
            }


        }

        [GCAuthorize(Roles = "SystemAdmin,SuperDocAdmin,documentadminid,documentauthorid")]
        public ActionResult StornoDocument(DocumentModel document, string HelpText)
        {
            DocumentFilter filter = (DocumentFilter)Session["DocumentFilter"];
            // TODO: LF - duvod storna dat do logu ?? zatim se ukládá  do sloupce StornoReason
            documentService.ChangeDocumentState(document, "Storno", HelpText);
            return RedirectToAction("Index", filter);
        }

        public ActionResult CancelChanges()
        {

            DocumentFilter filter = (DocumentFilter)Session["DocumentFilter"];
            return RedirectToAction("Index", filter);
        }

        [GCAuthorize(Roles = "SystemAdmin,SuperDocAdmin,documentadminid,documentauthorid")]
        public ActionResult SaveDocument(DocumentModel document, IEnumerable<HttpPostedFileBase> upload, string type, string HelpText)
        {
            // TODO: LF 1.11.2017 nutno doresit validaci 
            if (ModelState.IsValid)
            {
                DocumentFilter filter = (DocumentFilter)Session["DocumentFilter"];
                ICollection<string> fileNames = SaveFiles();
                int userId = UserRoles.GetUserId();


                if (type == "Add")
                {
                    document.IssueNumber = 1;
                    documentService.AddDocument(document, fileNames, userId);
                }
                else if (type == "Edit")
                {
                    documentService.EditDocument(document, fileNames);
                }

                return RedirectToAction("Index", filter);
            }

            //return View("Edit",document);
            if (document.ID==0){
                return Add(document.DocumentTypeID);
            } else
            {
                return Edit(document.ID);
            }
            


        }

        [GCAuthorizeAttribute(Roles = "SystemAdmin,SuperDocAdmin,documentadminid")]
        public ActionResult ArchivDocument(DocumentModel document, string HelpText)
        {

            DocumentFilter filter = (DocumentFilter)Session["DocumentFilter"];

            documentService.ChangeDocumentState(document, "Archived");
            
            return RedirectToAction("Index", filter);
        }

        [GCAuthorizeAttribute(Roles = "SystemAdmin,SuperDocAdmin,documentadminid")]
        public ActionResult DeArchivDocument(DocumentModel document, string HelpText)
        {

            DocumentFilter filter = (DocumentFilter)Session["DocumentFilter"];

            documentService.ChangeDocumentStateOnPreviousState(document, "");

            return RedirectToAction("Index", filter);
        }


        [GCAuthorizeAttribute(Roles = "SystemAdmin,SuperDocAdmin,documentadminid")]
        public ActionResult NewVersion(DocumentModel document)
        {
            DocumentFilter filter = (DocumentFilter)Session["DocumentFilter"];

            ICollection<string> fileNames = new HashSet<string>();

            // funguje
            //DocumentModel documentX = documentService.NewVersion(document, UserRoles.GetUserId(), fileNames);
            //return RedirectToAction("Edit", new { documentId = documentX.ID });

            // funguje
            document = documentService.NewVersion(document, UserRoles.GetUserId(), fileNames);

            return RedirectToAction("Edit", new { documentId = document.ID });

            // funguje
            // return RedirectToAction("Index", filter);


            // pada chybu
            // return RedirectToAction("Edit", documentX.ID);


            // totolni kolaps aplikace nepouzivat !!!!!
            //return Edit(documentX.ID);



        }

        [GCAuthorize(Roles = "SystemAdmin,SuperDocAdmin,documentadminid")]
        public ActionResult ReviewNoAction(DocumentModel document)
        {
            DocumentFilter filter = (DocumentFilter)Session["DocumentFilter"];

            documentService.ReviewNoAction(document);

            return RedirectToAction("Index", filter);
        }


        [GCAuthorize(Roles = "SystemAdmin,SuperDocAdmin,documentadminid")]
        public ActionResult ReviewNecessaryChange(DocumentModel document, string HelpText)
        {
            DocumentFilter filter = (DocumentFilter)Session["DocumentFilter"];

            documentService.ReviewNecessaryChange(document, HelpText, UserRoles.UserName());

            return RedirectToAction("Index", filter);
        }

        


        //[GCAuthorize (Roles = "DocumentAuthor")]
        //private ActionResult SaveInternal(DocumentModel document, IEnumerable<HttpPostedFileBase> upload, string type, string Action, string HelpText)
        //{
        //    DocumentFilter filter = (DocumentFilter)Session["DocumentFilter"];

        //    if (Action == "cancelChanges") return RedirectToAction("Index", filter);

        //    //if (Action == "cancelChanges") return RedirectToAction("Index", new { Message = "Dokument nebyl uložen." });

        //    ICollection<string> fileNames = SaveFiles();
        //    switch (type)
        //    {
        //        case "Add":
        //            // novy dokument ma vzdy cislo vydani 1
        //            switch (Action)
        //            {
        //                case "registerDocument":
        //                    document.IssueNumber = 1;
        //                    documentService.AddDocument(document, fileNames);
        //                    fileNames = null;
        //                    documentService.RegisterDocument(document, fileNames, UserRoles.GetUserId());
        //                    break;
        //                default:
        //                    document.IssueNumber = 1;
        //                    documentService.AddDocument(document, fileNames);
        //                    break;
        //            }

        //            break;

        //        case "Edit":

        //            switch (Action)
        //            {
        //                case "registerDocument":
        //                    documentService.RegisterDocument(document, fileNames, UserRoles.GetUserId());
        //                    break;
        //                case "newVersion":
        //                    documentService.NewVersion(document, UserRoles.GetUserId(), fileNames);
        //                    break;
        //                case "reviewNoAction":
        //                    documentService.ReviewNoAction(document);
        //                    break;
        //                case "reviewNecessaryChanges":

        //                    //UserRoles.UserName()
        //                    documentService.ReviewNecessaryChange(document, HelpText, UserRoles.UserName());
        //                    break;
        //                case "archivDocument":
        //                    //documentService.Archived(document, true);
        //                    documentService.ChangeDocumentState(document, "Archived");
        //                    break;
        //                case "deArchivDocument":
        //                    documentService.ChangeDocumentStateOnPreviousState(document, "");
        //                    //documentService.Archived(document, false);
        //                    break;
        //                case "stornoDocument":

        //                    documentService.ChangeDocumentState(document, "Storno");

        //                    break;

        //                case "cancelChanges":
        //                    break;
        //                default:
        //                    documentService.EditDocument(document, fileNames);
        //                    break;
        //            }


        //            break;
        //    }

        //    //return RedirectToAction("Index", new { Message = "Dokument byl uložen." });
        //    return RedirectToAction("Index", filter);
        //}

        private ICollection<string> SaveFiles()
        {
            string folderForFiles = System.Configuration.ConfigurationManager.AppSettings["FileTemp"];
            string guid = Guid.NewGuid().ToString();
            ICollection<string> fileNames = new HashSet<string>();
            string folderPath = folderForFiles + guid + @"\";
            Directory.CreateDirectory(folderPath);

            for (int i = 0; i < this.Request.Files.Count; i++)
            {
                if (this.Request.Files[i].ContentLength > 0)
                {
                    string strFileName = this.Request.Files[i].FileName
                        .Replace(":", string.Empty)
                        .Replace("'", string.Empty)
                        .Replace(";", string.Empty)
                        .Replace("*", string.Empty)
                        .Replace(@"/", string.Empty)
                        .Replace(@"\", string.Empty)
                        .Replace(">", string.Empty)
                        .Replace("<", string.Empty);
                    string filePath = folderPath + strFileName;
                    fileNames.Add(filePath);
                    this.Request.Files[i].SaveAs(filePath);
                }
            }

            return fileNames;
        }

        private void InitCodeLists()
        {

            ViewBag.Projects = codeListService.GetProjects();
            ViewBag.AppSystems = codeListService.GetAppSystems();
            ViewBag.Divisions = codeListService.GetDivisions();
            ViewBag.DocumentTypes = documentService.GetDocumentTypes();
            ViewBag.JobPositions = userService.GetJobPositions();
            ViewBag.Workplaces = codeListService.GetWorkplaces();
            ViewBag.Users = userService.GetUsers(new UserFilter());

        }
                
        [GCAuthorize (Roles = "SystemAdmin,SuperDocAdmin,DocAdmin,Author") ]
        public ActionResult Add(int documentTypeID)
        {

            int userId = UserRoles.GetUserId();

            // TODO: opravit ID = 1, DocumentStateCode, DocumentStateName na GetStateFromCode("New")
            //DocumentModel document = new DocumentModel() { Revision = "P", StateID = 1, IssueNumber = 1, DocumentStateCode = "New", DocumentStateName = "Nový" };
            DocumentModel document = new DocumentModel() { Revision = "P", IssueNumber = 1, DocumentStateCode = "New", DocumentStateName = "Nový"};
            ViewBag.Documents = document;

            
            // opraveno Lukas a Jane 25.7.2017
            ViewBag.Type = "Add";
            document.DocumentTypeID = documentTypeID;

            DocumentTypeModel typeModel = documentService.GetDocumentType(document.DocumentTypeID);
            ViewBag.TypeModel = typeModel;
            InitCodeLists();


            // LF  27.10.2017 - administraator id bude vzdy prednastavovan do noveho dokumentu
            // tj. pokud bude v typu dokumentu vyplnen
            document.AdministratorID = typeModel.AdministratorID ?? default(int);

            document.AuthorID = userId;

            //ICollection<Item> users = userService.GetUserList(new UserFilter() { });
            ICollection<Item> administrators = userService.GetUserList(new UserFilter() { });

            //ViewBag.Users = users.OrderBy(a => a.Value);

            if (document.AdministratorID == 0)
            {
                administrators.Add(new Item { ID = 0, Value = "---------"});
            }

            ViewBag.Administrators = administrators.OrderBy(a => a.Value);

            return View("edit", document);
        }

        [GCAuthorize(Roles = "SystemAdmin")]
        public ActionResult Delete(int documentId)
        {
            documentService.DeleteDocument(documentId);
            return RedirectToAction("Index", new { Message = "Dokument byl smazán." });
        }

        [GCAuthorize(Roles = "SystemAdmin,SuperDocAdmin,documentauthorid,documentadminid")]
        public ActionResult Edit(int documentId)
        {

            //int[] jobPositions = {};
            ////int[] usersID = {};
            ////int[] jobPositions = { 1, 2, 3 };
            //int[] usersID = { 12, 14 };

            //ICollection<UsersForJobPositionInDocumentModel> UsersForJobPositionInDocument =
            //documentService.GetUsersForJobPositionInDocument(documentId, jobPositions, usersID);


            //UserFilter filter = new UserFilter();
            //HashSet<int> jp = new HashSet<int>();
            //HashSet<int> uuu = new HashSet<int>();
            
            //jp.Add(2);
            //uuu.Add(3);

            //filter.JobPositionIDs = jp;
            //filter.UserIDs = uuu;

            //ICollection<UserJobModel> ujm = userService.GetUsersJob(filter);


            int userId = UserRoles.GetUserId();
            //ViewBag.Type = "Úprava řízeného dokumentu";
            DocumentModel document = documentService.GetDocument(documentId, userId);
            DocumentTypeModel typeModel = documentService.GetDocumentType(document.DocumentTypeID);
            ViewBag.TypeModel = typeModel;
            ViewBag.Documents = document;
            ViewBag.Type = "Edit";

            IEnumerable<Item> users = userService.GetUserList(new UserFilter() { }).OrderBy(u => u.Value);

            ViewBag.Administrators = users;
            //ViewBag.Users = users;

            InitCodeLists();
            return View("edit", document);
        }

        public ActionResult GetReadConfirms(int documentId, string selectedUserIDs, string selectedJobPositionIDs, string orderBy)
        {
            //int[] UserIDs = (!string.IsNullOrEmpty(selectedUserIDs)) ? Array.ConvertAll(selectedUserIDs.Split(','), int.Parse) : new int[] { };

            ICollection<int> UserIDs = ((!string.IsNullOrEmpty(selectedUserIDs)) ? Array.ConvertAll(selectedUserIDs.Split(','), int.Parse) : new int[] { }).ToList();
            ICollection<int> JoPostionIDs = ((!string.IsNullOrEmpty(selectedJobPositionIDs)) ? Array.ConvertAll(selectedJobPositionIDs.Split(','), int.Parse) : new int[] { }).ToList();

            ReadConfirmFilter filter = new ReadConfirmFilter();
            filter.DocumentID = documentId;
            filter.UserIDs = UserIDs;
            filter.JobPositionIDs = JoPostionIDs;
            filter.OrderBy = orderBy;

            ReadConfirmCollectionModel readConfirms = documentService.GetReadConfirms(filter);


            return Json(readConfirms, JsonRequestBehavior.AllowGet);
        }


        //public JobPositionModel GetJobPosition(int ID)
        //{
        //    return userService.GetJobPosition(ID);
        //}

    }
}