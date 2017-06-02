using System.Collections.Generic;
using GCPack.Model;

namespace GCPack.Repository.Interfaces
{
    public interface IDocumentsRepository
    {
        DocumentModel AddDocument(DocumentModel document);
        void AddFileToDb(Document document, string filePath);
        void DeleteDocument(Document document);
        void DeleteFileFromDocument(Document document, string filePath);
        Document EditDocument(Document document);
        ICollection<User> GetAddedUsersToDocument(ICollection<User> users, DocumentModel document);
        ICollection<User> GetDeletedUsersFromDocument(ICollection<User> users, DocumentModel document);
        void MapUsersToDocument(ICollection<User> addUsers, DocumentModel document, ICollection<User> deleteUsers);
    }
}