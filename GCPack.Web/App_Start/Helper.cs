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
        public static IDocumentsService GetInstance()
        {
            // rucne vytvorene injection objektu aby se nemuselo pouzivat IoC protoze ve staticke tride 
            // se neda vyuzit konstruktor

            return new DocumentsService(
                    new DocumentsRepository(),
                    new MailService(),
                    new UsersService(new UsersRepository(), new LogEventsService(new LogEventsRepository())),
                    new CodeListsService(new CodeListsRepository(), new LogEventsService(new LogEventsRepository())),
                    new LogEventsService(new LogEventsRepository())
                    );
        }

        // nacteni konkretniho dokumentu
        public static DocumentModel GetDocument(int documentId, int forUserId)
        {
            return GetInstance().GetDocuments(new DocumentFilter() { DocumentID = documentId, ForUserID = forUserId }).FirstOrDefault();
        }

        public static ICollection<Item> GetDocumentTypeItems()
        {
            return GetInstance().GetDocumentTypes();
        }

    }
}