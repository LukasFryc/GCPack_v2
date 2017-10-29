using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCPack.Repository.Interfaces;
using GCPack.Model;
using GCPack.Service.Interfaces;

namespace GCPack.Service
{
    public class CodeListsService : ICodeListsService
    {
        readonly ICodeListsRepository codeListsRepository;
        public CodeListsService(ICodeListsRepository codeListsRepository, ILogEventsService logEventsService)
        {
            this.codeListsRepository = codeListsRepository;
        }

        public ICollection<AppSystemModel> GetAppSystems()
        {
            return codeListsRepository.GetAppSystems();
        }

        public AppSystemModel GetAppSystem(int id)
        {
            return codeListsRepository.GetAppSystem(id);
        }

        public AppSystemModel AppSystemSave(AppSystemModel appSystem)
        {
            return codeListsRepository.AppSystemSave(appSystem);
        }

        public void AppSystemDelete(int id)
        {
            codeListsRepository.AppSystemDelete(id);
        }

        public ICollection<JobPositionModel> GetJobPositions()
        {
            return codeListsRepository.GetJobPositions();
        }

        public JobPositionModel GetJobPosition(int id)
        {
            return codeListsRepository.GetJobPosition(id);
        }

        public JobPositionModel JobPositionSave(JobPositionModel jobPosition)
        {
            return codeListsRepository.JobPositionSave(jobPosition);
        }

        public void JobPositionDelete(int id)
        {
            codeListsRepository.JobPositionDelete(id);
        }

        public ICollection<ProjectModel> GetProjects()
        {
            return codeListsRepository.GetProjects();
        }

        public ProjectModel GetProject(int id)
        {
            return codeListsRepository.GetProject(id);
        }

        public ProjectModel ProjectSave(ProjectModel project)
        {
            return codeListsRepository.ProjectSave(project);
        }

        public void ProjectDelete(int id)
        {
            codeListsRepository.ProjectDelete(id);
        }

        public ICollection<DivisionModel> GetDivisions()
        {
            return codeListsRepository.GetDivisions();
        }

        public DivisionModel GetDivision(int id)
        {
            return codeListsRepository.GetDivision(id);
        }

        public DivisionModel DivisionSave(DivisionModel division)
        {
            return codeListsRepository.DivisionSave(division);
        }

        public void DivisionDelete(int id)
        {
            codeListsRepository.DivisionDelete(id);
        }

        public ICollection<WorkplaceModel> GetWorkplaces()
        {
            return codeListsRepository.GetWorkplaces();
        }

        public WorkplaceModel GetWorkplace(int id)
        {
            return codeListsRepository.GetWorkplace(id);
        }

        public WorkplaceModel WorkplaceSave(WorkplaceModel workplace)
        {
            return codeListsRepository.WorkplaceSave(workplace);
        }

        public void WorkplaceDelete(int id)
        {
            codeListsRepository.WorkplaceDelete(id);
        }

        public ICollection<DocumentStateModel> GetDocumentStates()
        {
            return codeListsRepository.GetDocumentStates();
        }

        public void DocumentTypesDelete(int id)
        {
            codeListsRepository.DocumentTypeDelete(id);
        }
    }
}
