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
        readonly IUsersService userService;
        readonly ICodeListsService codeListService;

        public DocumentsService(IDocumentsRepository documentsRepository, IMailService mailService, IUsersService userService, ICodeListsService codeListService)
        {
            this.documentsRepository = documentsRepository;
            this.mailService = mailService;
            this.userService = userService;
            this.codeListService = codeListService;
        }
        

        public void RevisionNoAction(DocumentModel document, int userID, ICollection<string> fileNames)
        {
            DocumentModel documentModel = documentsRepository.GetDocument(document.ID, userID);
            documentModel.ReviewDate = DateTime.Now;
            DocumentTypeModel documentTypeModel = documentsRepository.GetDocumentType(document.DocumentTypeID);
            documentModel.NextReviewDate = DateTime.Now.AddYears(documentTypeModel.ValidityInYears);
            documentsRepository.EditDocument(documentModel);
            SaveFiles(document, fileNames);
        }

        public DocumentModel NewVersion(DocumentModel document, int userId, ICollection<string> fileNames)
        {
            // nacte se puvodni dokument

            DocumentModel oldDocument = GetDocument(document.ID, userId);
            document.ID = 0;

            // kontrola ze puvodni dokument je zaevidovany - pokud ne, tak se nedela nova verze

            // zvysi se issue number o 1
            document.IssueNumber = oldDocument.IssueNumber + 1;

            // zjisti se parentID noveho dokumentu = parentID puvodniho
            document.ParentID = oldDocument.ID;

            // novy dokument je ve stavu novy

            document.StateID = documentsRepository.GetDocumentState("New");

            // u stareho dokumentu se zmeni stav revize na R
            oldDocument.Revision = "R";

            // vymazani typu revize u noveho dokumentu
            document.Revision = "";

            // odeslani emailu vsem prirazenym osobam v dokumentu
            documentsRepository.ChangeRevison(oldDocument);
            //EditDocument(oldDocument, null);
            document = AddDocument(document, fileNames);
            
            // preulozit i soubory ???

            return new DocumentModel();
        }

        public string GenNumberOfDocument(int documentTypeID)
        {
            return documentsRepository.GenNumberOfDocument(documentTypeID);
        }

        public void SendEmail()
        {
            // metoda pro otestovani odesilani emailu
            int userId = 6;
            UserModel user = userService.GetUser(userId);
            DocumentModel document = documentsRepository.GetDocument(2015, userId);
            document.EffeciencyDate = DateTime.Now;

            mailService.SendEmail("TestovaciEmail", "Odeslani testovaciho emailu", user, document);
        }

        // zaevidovani dokumentu
        
        public DocumentModel RegisterDocument(DocumentModel document, ICollection<string> fileNames, int userID)
        {
            // dokument se nastavi na platny
            document.StateID = documentsRepository.GetDocumentState("Registered");

            // pokud se jedna o prvni verzi dokumentu, tak se vygeneruje se nove cislo dokumentu
            if (document.IssueNumber == 1)
            {
                document.DocumentNumber = GenNumberOfDocument(document.DocumentTypeID);
            }
            else
            {
                DocumentModel oldDocument = documentsRepository.GetDocument(document.ParentID, userID);
                document.DocumentNumber = oldDocument.DocumentNumber;
                oldDocument.Revision = "N";
                documentsRepository.EditDocument(oldDocument);
            }

            // stav revize dokumentu se prepne na P
            document.Revision = "P";

            // nastaveni datumu dalsi revize na: aktualni datum + pocet roku z revize dokumentu
            DocumentTypeModel documentType = GetDocumentType(document.DocumentTypeID);
            document.NextReviewDate = DateTime.Now.AddYears(documentType.ValidityInYears);

            // nastaveni ucinnosti dokumentu po schvaleni
            document.EffeciencyDate = DateTime.Now.AddDays(documentType.DocumentEfficiencyDays);

            // zmena revize dokumentu
            document = EditDocument(document, fileNames);

            documentsRepository.SetNumberOfDocument(document.DocumentTypeID);
            SaveFiles(document, fileNames);
            return document;
        }


        public DocumentTypeModel GetDocumentType(int ID)
        {
            DocumentTypeModel dm = documentsRepository.GetDocumentType(ID);
            if (dm.AdministratorID != null && dm.AdministratorID != 0)
            {
                var user = userService.GetUser((int) dm.AdministratorID);
                dm.AdministratorName = user.FirstName + " " + user.LastName;
            }
            return dm;
        }

        public DocumentTypeModel SaveDocumentType(DocumentTypeModel documentType)
        {
            return documentsRepository.SaveDocumentType(documentType);
        }

        public DocumentModel GetDocument(int documentId, int userID)
        {
            DocumentModel document = documentsRepository.GetDocument(documentId, userID);
            document.Users = documentsRepository.GetUsersForDocument(documentId);
            document.FileItems = documentsRepository.GetFiles(documentId);
            document.JobPositionIDs = documentsRepository.GetJobPositionsFromDocument(documentId);
            document.SelectedAppSystemsID = documentsRepository.GetAppSystemsFromDocument(documentId);
            document.SelectedProjectsID = documentsRepository.GetAppProjectsFromDocument(documentId);
            document.SelectedDivisionsID = documentsRepository.GetAppDivisionsFromDocument(documentId);
            document.SelectedWorkplacesID = documentsRepository.GetWorkplacesFromDocument(documentId);

            // nacteni uzivatelu 
            document.UsersInDocument = documentsRepository.GetUsersInDocument(documentId);

            return document;
        }

        public void ChangeDocumentState(DocumentModel document, string state)
        {
            documentsRepository.ChangeDocumentState(document, state);
        }
        public void ChangeDocumentState(int documentID, string state)
        {
            documentsRepository.ChangeDocumentState(documentID, state);
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
                documentsRepository.Readed(documentID, userID);
        }



        public DocumentModel AddDocument(DocumentModel document, ICollection<string> files)
        {

            // nastaveni stavu dokumentu na novy
            document.StateID = documentsRepository.GetDocumentState("New");

            // typ revize je vzdy u noveho dokumentu prazdny
            document.Revision = "";

            // novy dokument se vzdy uklada ve stavu rozpracovany
            // nikdy se u tohoto dokumentu neposilaji emaily

            document = documentsRepository.AddDocument(document);
            
            // namapuji se uzivatele na dokuemnt
            documentsRepository.MapUsersToDocument(document.SelectedUsers, document, null);

            SaveListCodes(document);

            // ulozeni vsech souboru
            SaveFiles(document, files);

            return new DocumentModel();

        }

        // ulozeni vsech ciselniku - spolecna fce pro add document a edit document
        private void SaveListCodes(DocumentModel document)
        {
            documentsRepository.SaveListCodes(document);
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

            // TODO: nacist vsechny soucasne jobpositions a porovnat ktere byly pridany
            // tem ktere byly pridany je potreba zaslat email

            documentsRepository.MapUsersToDocument(addedUsers.Select(au => au.ID).ToList(), document, deletedUsers.Select (u => u.ID).ToList<int>());
            documentsRepository.DeleteFilesFromDocument(document);
            document = documentsRepository.EditDocument(document);
            SaveListCodes(document);
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
