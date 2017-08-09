using System.Collections.Generic;
using GCPack.Model;

namespace GCPack.Service.Interfaces
{
    public interface ICodeListsService
    {
        ICollection<AppSystemModel> GetAppSystems();

        AppSystemModel GetAppSystem(int id);

        AppSystemModel AppSystemSave(AppSystemModel appSystem);

        void AppSystemDelete(int id);

    }
}