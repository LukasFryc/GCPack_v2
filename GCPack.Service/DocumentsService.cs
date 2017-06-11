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
        public DocumentModel GetDocument(int documentId, int userID)
        {
            DocumentModel document = documentsRepository.GetDocument(documentId, userID);
            document.Users = documentsRepository.GetUsersForDocument(documentId);
            document.FileItems = documentsRepository.GetFiles(documentId);
            return document;
        }

        public FileItem GetFile(int fileID)
        {
            return documentsRepository.GetFile(fileID);
        }

        public ICollection<DocumentModel> GetDocuments(DocumentFilter filter)
        {
            return documentsRepository.GetDocuments(filter);
        }

        public ICollection<Item> GetDocumentTypes()
        {
            return documentsRepository.GetDocumentTypes();
        }

        public void Readed(int documentID, int userID)
        {
            DocumentModel document = documentsRepository.GetDocument(documentID, userID);
            if (documentsRepository.ReadAccessToDocument(document, userID))
            {
                documentsRepository.Readed(document.ID, userID);
            }
            
        }

        public DocumentModel AddDocument(DocumentModel document, ICollection<string> files)
        {
            // novy dokument se vzdy uklada ve stavu rozpracovany
            // nikdy se u tohoto dokumentu neposilaji emaily

            document = documentsRepository.AddDocument(document);

            // namapuji se uzivatele na dokuemnt

            documentsRepository.MapUsersToDocument(document.SelectedUsers, document, null);

            // ulozeni vsech souboru
            SaveFiles(document, files);

            return new DocumentModel();

        }

        public void DeleteDocument(int documentId)
        {
            documentsRepository.DeleteDocument(documentId);
        }

        public DocumentModel EditDocument(DocumentModel document, ICollection<string> files)
        {
            // zjistit v jakem stavu je dokument pro posilani emailu pridanym nebo odstranenym uzivatelum

            // nacist uzivatele kteri se smazou
            ICollection<UserModel> deletedUsers =  documentsRepository.GetDeletedUsersFromDocument(document.SelectedUsers, document);

            // nacist uzivatele kteri se pridavaji

            ICollection<UserModel> addedUsers = documentsRepository.GetAddedUsersToDocument(document.SelectedUsers, document);


            documentsRepository.MapUsersToDocument(addedUsers.Select(au => au.ID).ToList(), document, deletedUsers.Select (u => u.ID).ToList<int>());
            documentsRepository.DeleteFilesFromDocument(document);
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
