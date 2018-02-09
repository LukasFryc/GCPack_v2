using System.Collections.Generic;
using GCPack.Model;

namespace GCPack.Repository.Interfaces
{
    public interface IDocumentsRepository
    {
        int GetDocumentState(string state);
        DocumentModel AddDocument(DocumentModel document);
        void AddFileToDb(DocumentModel document, byte[] fileStream, string name);
        void DeleteDocument(int documentId);
        void DeleteFilesFromDocument(DocumentModel document);
        DocumentModel EditDocument(DocumentModel document);
        ICollection<UserModel> GetAddedUsersToDocument(ICollection<int> users, DocumentModel document);
        ICollection<UserModel> GetDeletedUsersFromDocument(ICollection<int> users, DocumentModel document);
        void MapUsersToDocument(ICollection<int> addUsers, DocumentModel document, ICollection<int> deleteUsers);
        DocumentCollectionModel GetDocuments(DocumentFilter filter);
        DocumentModel GetDocument(int documentId, int? userID);
        ICollection<FileItem> GetFiles(int documentId);
        ICollection<UserModel> GetUsersForDocument(int documentId);
        FileItem GetFile(int fileID);
        void Readed(int documentID, int userID);
        bool ReadAccessToDocument(DocumentModel document, int userID);
        ICollection<Item> GetDocumentTypes();
        DocumentTypeModel GetDocumentType(int ID);
        DocumentTypeModel SaveDocumentType(DocumentTypeModel documentType);
        void ChangeDocumentState(DocumentModel document, string state);
        void ChangeDocumentState(DocumentModel document, string state,string helpText);
        void ChangeDocumentState(int documentID, string state);
        void ChangeDocumentState(int documentID, string state, string helpText);
        ICollection<int> GetJobPositionsFromDocument(int documentId);
        ICollection<DocumentModel> GetDocuments_priklad(DocumentFilter filter);
        string GenNumberOfDocument(int documentTypeID);
        void SetNumberOfDocument(int documentTypeID);
        ICollection<int> GetAppSystemsFromDocument(int documentId);
        ICollection<int> GetAppProjectsFromDocument(int documentId);
        ICollection<int> GetAppDivisionsFromDocument(int documentId);

        ICollection<int> GetWorkplacesFromDocument(int documentId);
        void SaveListCodes(DocumentModel document);
        UsersInDocument GetUsersInDocument(int documentID);
        void ChangeRevison(DocumentModel document, string revisionType);

        //void Archived(DocumentModel document, bool archiv);

        //void Archived(int documentId, bool archiv);

        void ReviewNoAction(DocumentModel document);

        void ReviewNecessaryChange(DocumentModel document, string comment, string userName);

        void ChangeDocumentStateOnPreviousState(DocumentModel document, string newState);
        //void ReadedUserInFunctions(int confirmID, int jobPositionID, string name);

        // TODO: LF prezenatce rozdelovniku v tabulce 
        //ICollection<UsersForJobPositionInDocumentModel> GetUsersForJobPositionInDocument(int documentId, ICollection<int> jobPositionsID, ICollection<int> usersID);

        void AddReadConfirms(int documentID, ICollection<UserJobModel> usersJob);
        ReadConfirmCollectionModel GetReadConfirms(ReadConfirmFilter  filter);

        ICollection<Item> GetUniqueAuthorsDocuments(DocumentFilter filter);

        ICollection<Item> GetUniqueAdministratorsDocuments(DocumentFilter filter);

        ICollection<Item> GetUniqueReadConfirmsDocuments(DocumentFilter filter);

        DocumentCollectionModel GetDocuments_linq(DocumentFilter filter);

    }
}