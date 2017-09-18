using System.Collections.Generic;
using GCPack.Model;

namespace GCPack.Service.Interfaces
{
    public interface IDocumentsService
    {
        DocumentModel AddDocument(DocumentModel document, ICollection<string> files);
        DocumentModel EditDocument(DocumentModel document, ICollection<string> files);
        DocumentModel GetDocument(int documentId, int userID);
        ICollection<DocumentModel> GetDocuments(DocumentFilter filter);
        void DeleteDocument(int documentId);
        FileItem GetFile(int fileID);
        void Readed(int documentID, int userID);
        ICollection<Item> GetDocumentTypes();
        DocumentTypeModel GetDocumentType(int ID);
        DocumentTypeModel SaveDocumentType(DocumentTypeModel documentType);
        void ChangeDocumentState(DocumentModel document, string state);
        void ChangeDocumentState(int documentID, string state);
        void SendEmail();
        string GenNumberOfDocument(int documentTypeID);
        DocumentModel RegisterDocument(DocumentModel document, ICollection<string> fileNames, int userID);
        DocumentModel NewVersion(DocumentModel document, int userId, ICollection<string> fileNames);
        void ReviewNoAction(DocumentModel document, int userId, ICollection<string> fileNames);
        void ReviewNecessaryChange(DocumentModel document, int userID, ICollection<string> fileNames);
    }
}