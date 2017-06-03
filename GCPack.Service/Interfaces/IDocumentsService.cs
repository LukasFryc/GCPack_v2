using System.Collections.Generic;
using GCPack.Model;

namespace GCPack.Service.Interfaces
{
    public interface IDocumentsService
    {
        DocumentModel AddDocument(DocumentModel document, ICollection<string> files);
        DocumentModel EditDocument(DocumentModel document);
        DocumentModel GetDocument(int documentId);
    }
}