using System.Collections.Generic;
using GCPack.Model;

namespace GCPack.Repository.Interfaces
{
    public interface ICodeListsRepository
    {
        ICollection<AppSystemModel> GetAppSystems();

        AppSystemModel GetAppSystem(int id);

        AppSystemModel AppSystemSave(AppSystemModel appSystem);

        void AppSystemDelete(int id);
    }
}