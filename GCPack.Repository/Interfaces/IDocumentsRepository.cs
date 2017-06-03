using System.Collections.Generic;
using GCPack.Model;

namespace GCPack.Repository.Interfaces
{
    public interface IDocumentsRepository
    {
        DocumentModel AddDocument(DocumentModel document);
        void AddFileToDb(DocumentModel document, string filePath);
        void DeleteDocument(DocumentModel document);
        void DeleteFileFromDocument(DocumentModel document, string filePath);
        DocumentModel EditDocument(DocumentModel document);
        ICollection<UserModel> GetAddedUsersToDocument(ICollection<int> users, DocumentModel document);
        ICollection<UserModel> GetDeletedUsersFromDocument(ICollection<int> users, DocumentModel document);
        void MapUsersToDocument(ICollection<int> addUsers, DocumentModel document, ICollection<int> deleteUsers);
    }
}