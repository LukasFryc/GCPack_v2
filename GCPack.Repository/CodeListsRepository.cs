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
                return Mapper.Map<ICollection<AppSystemModel>>(db.AppSystems.Select(ap => ap));
            }
        }


        public AppSystemModel GetAppSystem(int id)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                return Mapper.Map<AppSystemModel>(db.AppSystems.Where(dt => dt.ID == id).Select(dt => dt).FirstOrDefault());
                // kupodivu niye uvedene nefunguje
                //return Mapper.Map<AppSystemModel>(db.AppSystems.Where(ap => ap.Equals(id)).Select(ap => ap).FirstOrDefault());
            }
        }

        public AppSystemModel AppSystemSave(AppSystemModel appSystem)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                if (appSystem.ID == 0)
                {
                    var dbAppSystem = db.AppSystems.Add(Mapper.Map<AppSystem>(appSystem));
                    db.SaveChanges();
                    appSystem.ID = dbAppSystem.ID;
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

    }
}
