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
        readonly ICodeListsService codeListsService;
        public CodeListsController(IDocumentsService documentsService, IUsersService usersService, ICodeListsService codeListsService)
        {
            this.documentsService = documentsService;
            this.usersService = usersService;
            this.codeListsService = codeListsService;
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

        /// SYSTEM
        /// 

        public ActionResult AppSystemIndex()
        {
            ICollection<AppSystemModel> appSystem = codeListsService.GetAppSystems();
            return View(appSystem);
        }

        
      public ActionResult AppSystemEdit(int ID)
      {
          ViewBag.Title = "Editace číselníku systém";
          AppSystemModel appSystem = codeListsService.GetAppSystem(ID);
          return View(appSystem);
      }
       
        public ActionResult AppSystemAdd()
        {
            ViewBag.Title = "Nová položka číselníku systém";
            return View("AppSystemEdit", new AppSystemModel());
        }
            

        public ActionResult AppSystemSave(AppSystemModel appSystem)
        {
            codeListsService.AppSystemSave(appSystem);
            return RedirectToAction("AppSystemIndex");
        }

        public ActionResult AppSystemDelete(int id)
        {
            codeListsService.AppSystemDelete(id);
            return RedirectToAction("AppSystemIndex");
        }

        // JOBPOSITIONS
        public ActionResult JobPositionIndex()
        {
            ICollection<JobPositionModel> jobPosition = codeListsService.GetJobPositions();
            return View(jobPosition);
        }

        public ActionResult JobPositionEdit(int ID)
        {
            ViewBag.Title = "Editace číselníku pracovních pozic";
            JobPositionModel jobPosition = codeListsService.GetJobPosition(ID);
            return View(jobPosition);
        }

        public ActionResult JobPositionAdd()
        {
            ViewBag.Title = "Nová položka číselníku systém";
            return View("JobPositionEdit", new JobPositionModel());
        }


        public ActionResult JobPositionSave(JobPositionModel jobPosition)
        {
            codeListsService.JobPositionSave(jobPosition);
            return RedirectToAction("JobPositionIndex");
        }

        public ActionResult JobPositionDelete(int id)
        {
            codeListsService.JobPositionDelete(id);
            return RedirectToAction("JobPositionIndex");
        }

        // PROJECTS
        public ActionResult ProjectIndex()
        {
            ICollection<ProjectModel> projects = codeListsService.GetProjects();
            return View(projects);
        }

        public ActionResult ProjectEdit(int ID)
        {
            ViewBag.Title = "Editace číselníku projektu";
            ProjectModel project = codeListsService.GetProject(ID);
            return View(project);
        }

        public ActionResult ProjectAdd()
        {
            ViewBag.Title = "Nová položka projektů";
            return View("ProjectEdit", new ProjectModel());
        }


        public ActionResult ProjectSave(ProjectModel project)
        {
            codeListsService.ProjectSave(project);
            return RedirectToAction("ProjectIndex");
        }

        public ActionResult ProjectDelete(int id)
        {
            codeListsService.ProjectDelete(id);
            return RedirectToAction("ProjectIndex");
        }

        // DIVISION
        public ActionResult DivisionIndex()
        {
            ICollection<DivisionModel> divisions = codeListsService.GetDivisions();
            return View(divisions);
        }

        public ActionResult DivisionEdit(int ID)
        {
            ViewBag.Title = "Editace číselníku střediska";
            DivisionModel division = codeListsService.GetDivision(ID);
            return View(division);
        }

        public ActionResult DivisionAdd()
        {
            ViewBag.Title = "Nová položka střediska";
            return View("DivisionEdit", new DivisionModel());
        }


        public ActionResult DivisionSave(DivisionModel division)
        {
            codeListsService.DivisionSave(division);
            return RedirectToAction("DivisionIndex");
        }

        public ActionResult DivisionDelete(int id)
        {
            codeListsService.DivisionDelete(id);
            return RedirectToAction("DivisionIndex");
        }

        public ActionResult WorkplaceIndex()
        {
            ICollection<WorkplaceModel> workplaces = codeListsService.GetWorkplaces();
            return View(workplaces);
        }
        public ActionResult WorkplaceEdit(int ID)
        {
            ViewBag.Title = "Editace číselníku pracovní místa";
            WorkplaceModel workplace = codeListsService.GetWorkplace(ID);
            return View(workplace);
        }

        public ActionResult WorkplaceAdd()
        {
            ViewBag.Title = "Nová položka pracovního místa";
            return View("WorkplaceEdit", new WorkplaceModel());
        }


        public ActionResult WorkplaceSave(WorkplaceModel workplace)
        {
            codeListsService.WorkplaceSave(workplace);
            return RedirectToAction("WorkplaceIndex");
        }

        public ActionResult WorkplaceDelete(int id)
        {
            codeListsService.WorkplaceDelete(id);
            return RedirectToAction("WorkplaceIndex");
        }

    }
}
