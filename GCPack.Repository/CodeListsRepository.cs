using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCPack.Repository.Interfaces;
using GCPack.Model;
using AutoMapper;

namespace GCPack.Repository
{
    public class CodeListsRepository : ICodeListsRepository
    {
        public ICollection<AppSystemModel> GetAppSystems()
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                return Mapper.Map<ICollection<AppSystemModel>>(db.AppSystems.Select(ap => ap).OrderBy(ap => ap.Name));
            }
        }


        public AppSystemModel GetAppSystem(int id)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                return Mapper.Map<AppSystemModel>(db.AppSystems.Where(dt => dt.ID == id).Select(dt => dt).FirstOrDefault());
            }
        }

        public AppSystemModel AppSystemSave(AppSystemModel appSystem)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                if (appSystem.ID == 0)
                {
                    var dbCodeList = db.AppSystems.Add(Mapper.Map<AppSystem>(appSystem));
                    db.SaveChanges();
                    appSystem.ID = dbCodeList.ID;
                }
                else
                {
                    var dbAppSystem = db.AppSystems.Where(dt => dt.ID == appSystem.ID).FirstOrDefault();
                    Mapper.Map(appSystem, dbAppSystem);
                    db.SaveChanges();
                }
            }
            return appSystem;
        }

        public void AppSystemDelete(int id)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                db.AppSystems.RemoveRange(db.AppSystems.Where(ap => ap.ID == id));
                db.SaveChanges();
            }
        }

        public ICollection<JobPositionModel> GetJobPositions()
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                return Mapper.Map<ICollection<JobPositionModel>>(db.JobPositions.Select(jp => jp).OrderBy(jp => jp.Name));
            }
        }

        public JobPositionModel GetJobPosition(int id)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                return Mapper.Map<JobPositionModel>(db.JobPositions.Where(jp => jp.ID == id).Select(jp => jp).FirstOrDefault());
            }
        }

        public JobPositionModel JobPositionSave(JobPositionModel jobPosition)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                if (jobPosition.ID == 0)
                {
                    var dbCodeList = db.JobPositions.Add(Mapper.Map<JobPosition>(jobPosition));
                    db.SaveChanges();
                    jobPosition.ID = dbCodeList.ID;
                }
                else
                {
                    var dbCodeList = db.JobPositions.Where(dt => dt.ID == jobPosition.ID).FirstOrDefault();
                    Mapper.Map(jobPosition, dbCodeList);
                    db.SaveChanges();
                }
            }
            return jobPosition;
        }

        public void JobPositionDelete(int id)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                db.JobPositions.RemoveRange(db.JobPositions.Where(ap => ap.ID == id));
                db.SaveChanges();


            }
        }

        public ICollection<ProjectModel> GetProjects()
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                return Mapper.Map<ICollection<ProjectModel>>(db.Projects.Select(p => p).OrderBy(p => p.Name));
            }
        }

        public ProjectModel GetProject(int id)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                return Mapper.Map<ProjectModel>(db.Projects.Where(p => p.ID == id).Select(p => p).FirstOrDefault());
            }
        }

        public ProjectModel ProjectSave(ProjectModel project)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                if (project.ID == 0)
                {
                    var dbCodeList = db.Projects.Add(Mapper.Map<Project>(project));
                    db.SaveChanges();
                    project.ID = dbCodeList.ID;
                }
                else
                {
                    var dbCodeList = db.Projects.Where(dt => dt.ID == project.ID).FirstOrDefault();
                    Mapper.Map(project, dbCodeList);
                    db.SaveChanges();
                }
            }
            return project;
        }

        public void ProjectDelete(int id)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                db.Projects.RemoveRange(db.Projects.Where(ap => ap.ID == id));
                db.SaveChanges();


            }
        }

        public ICollection<DivisionModel> GetDivisions()
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                return Mapper.Map<ICollection<DivisionModel>>(db.Divisions.Select(d => d).OrderBy(d => d.Name));
            }
        }

        public DivisionModel GetDivision(int id)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                return Mapper.Map<DivisionModel>(db.Divisions.Where(d => d.ID == id).Select(d => d).FirstOrDefault());
            }
        }
        
        public DivisionModel DivisionSave(DivisionModel division)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                if (division.ID == 0)
                {
                    var dbCodeList = db.Divisions.Add(Mapper.Map<Division>(division));
                    db.SaveChanges();
                    division.ID = dbCodeList.ID;
                }
                else
                {
                    var dbCodeList = db.Divisions.Where(d => d.ID == division.ID).FirstOrDefault();
                    Mapper.Map(division, dbCodeList);
                    db.SaveChanges();
                }
            }
            return division;
        }

        public void DivisionDelete(int id)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                db.Divisions.RemoveRange(db.Divisions.Where(d => d.ID == id));
                db.SaveChanges();


            }
        }
    }
}
