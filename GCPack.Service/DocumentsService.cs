using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCPack.Model;
using GCPack.Repository.Interfaces;
using GCPack.Service.Interfaces;

namespace GCPack.Service
{
    public class DocumentsService : IDocumentsService
    {
        readonly IDocumentsRepository documentsRepository;
        readonly IMailService mailService;

        public DocumentsService(IDocumentsRepository documentsRepository, IMailService mailService)
        {
            this.documentsRepository = documentsRepository;
            this.mailService = mailService;
        }
        public DocumentModel GetDocument(int documentId)
        {
            return new DocumentModel();
        }


        public DocumentModel AddDocument(DocumentModel document, ICollection<string> files)
        {
            // novy dokument se vzdy uklada ve stavu rozpracovany
            // nikdy se u tohoto dokumentu neposilaji emaily

            document = documentsRepository.AddDocument(document);

            // namapuji se uzivatele na dokuemnt

            documentsRepository.MapUsersToDocument( document.SelectedUsers, document, null);

            // ulozeni vsech souboru
            foreach (string file in files)
            {
                try
                {
                    documentsRepository.AddFileToDb(document, file);
                }
                catch (Exception e)
                { }
            }


            return new DocumentModel();

        }

        public DocumentModel EditDocument(DocumentModel document)
        {
            // zjistit v jakem stavu je dokument pro posilani emailu pridanym nebo odstranenym uzivatelum

            // nacist uzivatele kteri se smazou

            // nacist uzivatele kteri se pridavaji



            return new DocumentModel();

        }

    }
}
