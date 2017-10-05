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
        private readonly ICodeListsService codeListService;
        public DocumentsController(IDocumentsService documentService, IUsersService userService, ICodeListsService codeListService)
        {
            this.documentService = documentService;
            this.userService = userService;
            this.codeListService = codeListService;
        }

        [AuthorizeAttributeGC(Roles = "documentOwner,documentAdmin")]
        public ActionResult Edit2(int DocumentID, string Message)
        {
            return View();
        }


        // GET: Documents
        [AuthorizeAttributeGC(Roles = "user,admin,supervisor,poweruser")]
        public ActionResult Index(int? DocumentTypeID, string Message)
        {
            // TODO: dopsat filtrovani - pridat do filtru userId pro ktereho se vyberou pouze jeho dokumenty
            ICollection<DocumentModel> documents = new HashSet<DocumentModel>();

            ICollection<Item> documentTypes = documentService.GetDocumentTypes();
            

            if (DocumentTypeID != null) {

                // TODO: Zvazit upravu cislenikovych trid o dedeni z tridy item
                // TODO: Napreklad pri volani funkce documentService.GetDocumentTypes se varci IColection<Item>
                // TODO: zatimco documentService.GetDocumentType vraci DocumentTypeModel
                // TODO: zvazit zda ciselnikove tridy by nemel dedti z obecne tridy Item, je zde samozrejme problem 
                // TODO: s jiz stavajicim pouzitim trid a s rozdily v propertach (Value x Name a orderBy x OrderBy) 
                // TODO: mozna se to budoucnu toto precovani muze hodit - mohl bych pretypovat DocumentTypeModel nebo ProjectModel nebo DivisionMOdel na Item


                // int v2 = 0;
                // if (DocumentTypeID.HasValue) v2 = DocumentTypeID.Value;
                //DocumentTypeModel  choiceDocumentType = documentService.GetDocumentType(v2);

                Item choiceDocumentType = documentTypes.Where(dc => dc.ID == DocumentTypeID).Select(dc => dc).FirstOrDefault();
               choiceDocumentType.OrderBy = -1;
            }

            //ICollection<Item> documentSortType = new HashSet<Item>(); ;
            //documentSortType.Add(new Item { ID = 0, Code = "",  OrderBy = 0, Value = "Sestupně" });

            //ICollection<Item> documentSort = new HashSet<Item>(); ;
            //documentSort.Add(new Item { ID = 0, Code = "", OrderBy = 0, Value = "Setřídit podle" });
            //documentSort.Add(new Item { ID = 1, Code = "Title", OrderBy = 0, Value = "Názvu" });
            //documentSort.Add(new Item { ID = 2, Code = "DocumentNumber", OrderBy = 0, Value = "Čísla" });
            //documentSort.Add(new Item { ID = 3, Code = "DocumentAdminType", OrderBy = 0, Value = "Správce" });
            //documentSort.Add(new Item { ID = 4, Code = "EffeciencyDate", OrderBy = 0, Value = "Data účinnosti" });
            //ViewBag.DocumentSort = documentSort.OrderBy(dt => dt.OrderBy);

            ICollection<Item> archivType = new HashSet<Item>(); ;
            archivType.Add(new Item { ID = 0, Code = "all", OrderBy = 2, Value = "Všechny" });
            archivType.Add(new Item { ID = 1, Code = "1", OrderBy = 1, Value = "Je v archívu" });
            archivType.Add(new Item { ID = 2, Code = "0", OrderBy = 0, Value = "Není v archívu" });


            //ICollection<UserModel> users = new HashSet<UserModel>();
            //users = userService.GetUsers(new UserFilter());
            //users.Add(new UserModel() { ID = 0, Name = "Zvolte osobu", orderBy = 1 });
            //ViewBag.Projects = users.OrderByDescending(p => p.orderBy).ThenBy(p => p.Name);

            ICollection<Item> revisionType = new HashSet<Item>(); ;
            revisionType.Add(new Item { ID = 0, Code = "all", OrderBy = 0, Value = "Všechny" });
            revisionType.Add(new Item { ID = 1, Code = "p", OrderBy = 1, Value = "Platné" });
            revisionType.Add(new Item { ID = 2, Code = "n", OrderBy = 2, Value = "Neplatné" });
            revisionType.Add(new Item { ID = 3, Code = "r", OrderBy = 3, Value = "V revizi" });
            ViewBag.RevisionType = revisionType.OrderBy(rt => rt.OrderBy);

            ICollection<Item> readType = new HashSet<Item>(); ;
            readType.Add(new Item { ID = 0, Code = "all", OrderBy = 0, Value = "Všechny" });
            readType.Add(new Item { ID = 2, Code = "read", OrderBy = 1, Value = "Seznámeno" });
            readType.Add(new Item { ID = 1, Code = "unread", OrderBy = 2, Value = "Neseznámeno" });
            //readType.Add(new Item { ID = 3, Code = "unreadafterterm", OrderBy = 3, Value = "Neseznámeno po termínu" });
            ViewBag.ReadType = readType.OrderBy(dt => dt.OrderBy);

            documentTypes.Add(new Item { ID = 0, OrderBy = 0, Value = "Všechny dokumenty" });
            ViewBag.DocumentTypes = documentTypes.OrderBy(dt => dt.OrderBy);

            ICollection<DocumentStateModel> documentStates = new HashSet<DocumentStateModel>();
            documentStates = codeListService.GetDocumentStates();
            documentStates.Add(new DocumentStateModel() { ID = 0, Name = "Všechny stavy", orderBy = -1 });
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

            ViewBag.Message = Message;
            return View(documents);
            //DocumentFilter filterModel = new DocumentFilter() { WorkplaceID = 1, AppSystemID = 1, StateID = 2 };
            //return View(filterModel);

        }

        public void SendMail()
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

        public ActionResult Registered(int documentID)
        {
            documentService.ChangeDocumentState(documentID, "Registered");
            return View();
        }

        public ActionResult GetDocuments(DocumentFilter filter)
        {

           // filter.EffeciencyDateFrom = string.IsNullOrEmpty(filter.EffeciencyDateFrom.ToString())
           //? (DateTime?)null
           //: DateTime.Parse(filter.EffeciencyDateFrom.ToString());

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

        public ActionResult Save(DocumentModel document, IEnumerable<HttpPostedFileBase> upload, string type, string Action, string HelpText)
        {

            if (Action == "cancelChanges") return RedirectToAction("Index", new { Message = "Dokument nebyl uložen." });

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
                    // novy dokument ma vzdy cislo vydani 1
                    switch (Action)
                    {
                        case "registerDocument":
                            document.IssueNumber = 1;
                            documentService.AddDocument(document, fileNames);
                            documentService.RegisterDocument(document, fileNames, UserRoles.GetUserId());
                            break;
                        case "cancelChanges":
                            break;
                        default:
                            document.IssueNumber = 1;
                            documentService.AddDocument(document, fileNames);
                            break;
                    }

                    break;

                case "Edit":

                    switch (Action)
                    {
                        case "registerDocument":
                            documentService.RegisterDocument(document, fileNames, UserRoles.GetUserId());
                            break;
                        case "newVersion":
                            documentService.NewVersion(document, UserRoles.GetUserId(), fileNames);
                            break;
                        case "reviewNoAction":
                            documentService.ReviewNoAction(document);
                            break;
                        case "reviewNecessaryChanges":

                            //UserRoles.UserName()
                            documentService.ReviewNecessaryChange(document, HelpText, UserRoles.UserName());
                            break;
                        case "archivDocument":
                            //documentService.Archived(document, true);
                            documentService.ChangeDocumentState(document, "Archived");
                            break;
                        case "deArchivDocument":
                            documentService.ChangeDocumentState(document, "Registred");
                            //documentService.Archived(document, false);
                            break;
                        case "stornoDocument":

                            documentService.ChangeDocumentState(document, "Storno");

                            break;

                        case "cancelChanges":
                            break;
                        default:
                            documentService.EditDocument(document, fileNames);
                            break;
                    }

                       
                    break;
            }
            
            return RedirectToAction("Index", new { Message = "Dokument byl uložen." });
        }


        private void InitCodeLists()
        {
            ViewBag.Projects = codeListService.GetProjects();
            ViewBag.AppSystems = codeListService.GetAppSystems();
            ViewBag.Divisions = codeListService.GetDivisions();
            ViewBag.DocumentTypes = documentService.GetDocumentTypes();
            ViewBag.JobPositions = userService.GetJobPositions();
            ViewBag.Workplaces = codeListService.GetWorkplaces();

        }

        public ActionResult Add(int documentTypeID)
        {
            ViewBag.Type = "Nový řízený dokument";
            // TODO: opravit ID = 1, DocumentStateCode, DocumentStateName na GetStateFromCode("New")
            DocumentModel document = new DocumentModel() { Revision = "P", StateID = 1, IssueNumber = 1, DocumentStateCode = "New", DocumentStateName = "Nový"};
            ViewBag.Documents = document;
            ViewBag.Administrators = userService.GetUserList(new UserFilter() { });
            // opraveno Lukas a Jane 25.7.2017
            ViewBag.Type = "Add";
            document.DocumentTypeID = documentTypeID;
            DocumentTypeModel typeModel = documentService.GetDocumentType(document.DocumentTypeID);
            ViewBag.TypeModel = typeModel;
            InitCodeLists();

            return View("edit",document);
        }

        public ActionResult Delete(int documentId)
        {
            documentService.DeleteDocument(documentId);
            return RedirectToAction("Index", new { Message = "Dokument byl smazán." });
        }

        public ActionResult Edit(int documentId)
        {

            int userId = UserRoles.GetUserId();
            ViewBag.Type = "Úprava řízeného dokumentu";
            DocumentModel document = documentService.GetDocument(documentId, userId);
            DocumentTypeModel typeModel = documentService.GetDocumentType(document.DocumentTypeID);
            ViewBag.TypeModel = typeModel;
            ViewBag.Documents = document;
            ViewBag.Administrators = userService.GetUserList(new UserFilter() { });
            ViewBag.Type = "Edit";
            //ViewBag.ComeBackFilter = 
            InitCodeLists();
            return View("edit", document);
        }

    }
}