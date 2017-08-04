using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GCPack.Repository;
using GCPack.Service;
using GCPack.Service.Interfaces;
using GCPack.Model;

namespace GCPack
{
    public static class Helper
    {
        public static ICollection<Item> GetDocumentTypeItems()
        {
            // rucne vytvorene injection objektu aby se nemuselo pouzivat IoC protoze ve staticke tride 
            // se neda vyuzit konstruktor
            IDocumentsService documentService = 
                new DocumentsService(new DocumentsRepository(), new MailService(), new UsersService( new UsersRepository() ));
            return documentService.GetDocumentTypes();
        }
        
    }
}