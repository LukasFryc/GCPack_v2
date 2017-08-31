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
        ICollection<DocumentModel> GetDocuments(DocumentFilter filter);
        DocumentModel GetDocument(int documentId, int userID);
        ICollection<FileItem> GetFiles(int documentId);
        ICollection<UserModel> GetUsersForDocument(int documentId);
        FileItem GetFile(int fileID);
        void Readed(int documentID, int userID);
        bool ReadAccessToDocument(DocumentModel document, int userID);
        ICollection<Item> GetDocumentTypes();
        DocumentTypeModel GetDocumentType(int ID);
        DocumentTypeModel SaveDocumentType(DocumentTypeModel documentType);
        void ChangeDocumentState(DocumentModel document, string state);
        void ChangeDocumentState(int documentID, string state);
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
        void ChangeRevison(DocumentModel document);
    }
}