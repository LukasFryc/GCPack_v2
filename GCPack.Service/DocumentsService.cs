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
            DocumentModel document  = documentsRepository.GetDocument(documentId);
            document.FileItems = documentsRepository.GetFiles(documentId);
            return document;
        }

        public ICollection<DocumentModel> GetDocuments(DocumentFilter filter)
        {
            return documentsRepository.GetDocuments(filter);
        }

        public DocumentModel AddDocument(DocumentModel document, ICollection<string> files)
        {
            // novy dokument se vzdy uklada ve stavu rozpracovany
            // nikdy se u tohoto dokumentu neposilaji emaily
            
            document = documentsRepository.AddDocument(document);

            // namapuji se uzivatele na dokuemnt

            documentsRepository.MapUsersToDocument( document.SelectedUsers, document, null);

            // ulozeni vsech souboru
            SaveFiles(document, files);

            return new DocumentModel();

        }

        public DocumentModel EditDocument(DocumentModel document, ICollection<string> files)
        {
            // zjistit v jakem stavu je dokument pro posilani emailu pridanym nebo odstranenym uzivatelum

            // nacist uzivatele kteri se smazou
            ICollection<UserModel> deletedUsers =  documentsRepository.GetDeletedUsersFromDocument(document.SelectedUsers, document);

            // nacist uzivatele kteri se pridavaji
    
            documentsRepository.MapUsersToDocument(document.SelectedUsers, document, deletedUsers.Select (u => u.ID).ToList<int>());

            document = documentsRepository.EditDocument(document);

            SaveFiles(document, files);

            return document;

        }

        private void SaveFiles(DocumentModel document, ICollection<string> files)
        {
            if (files != null && files.Count > 0)
            {
                foreach (string file in files)
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(file);
                    documentsRepository.AddFileToDb(document, System.IO.File.ReadAllBytes(file), fi.Name);
                }
            }
        }

    }
}
