using GCPack.Model;

namespace GCPack.Service.Interfaces
{
    public interface IMailService
    {
        void SendEmail(string from, UserModel to, DocumentModel document, string template);
    }
}