using GCPack.Model;

namespace GCPack.Service.Interfaces
{
    public interface IDocumentsService
    {
        RizenyDokument GetDocument(int documentId);
    }
}