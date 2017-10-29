using System.Collections.Generic;
using GCPack.Model;

namespace GCPack.Repository.Interfaces
{
    public interface ICodeListsRepository
    {
        // AppSystem
        ICollection<AppSystemModel> GetAppSystems();

        AppSystemModel GetAppSystem(int id);

        AppSystemModel AppSystemSave(AppSystemModel appSystem);

        void AppSystemDelete(int id);


        // JobPosition
        ICollection<JobPositionModel> GetJobPositions();

        JobPositionModel GetJobPosition(int id);

        JobPositionModel JobPositionSave(JobPositionModel jobPosition);

        void JobPositionDelete(int id);

        // Project
        ICollection<ProjectModel> GetProjects();

        ProjectModel GetProject(int id);

        ProjectModel ProjectSave(ProjectModel project);

        void ProjectDelete(int id);

        // Division
        ICollection<DivisionModel> GetDivisions();

        DivisionModel GetDivision(int id);

        DivisionModel DivisionSave(DivisionModel division);

        void DivisionDelete(int id);

        // Workplace
        ICollection<WorkplaceModel> GetWorkplaces();

        WorkplaceModel GetWorkplace(int id);

        WorkplaceModel WorkplaceSave(WorkplaceModel workplace);

        void WorkplaceDelete(int id);

        ICollection<DocumentStateModel> GetDocumentStates();

        void DocumentTypeDelete(int id);
        

    }
}