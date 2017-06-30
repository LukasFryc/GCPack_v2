using GCPack.Model;

namespace GCPack.Service.Interfaces
{
    public interface IMailService
    {
        void SendEmail(string subject, string body, string to);
        void SendEmail(string template, string subject, UserModel user, DocumentModel document);
    }
}