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
        public CodeListsService(ICodeListsRepository codeListsRepository)
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

    }
}
