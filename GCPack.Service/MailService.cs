using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCPack.Model;
using GCPack.Service.Interfaces;
using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace GCPack.Service
{
    public class MailService : IMailService
    {
        SmtpClient smtpClient = new SmtpClient();
        NetworkCredential basicCredential =
            new NetworkCredential(ConfigurationManager.AppSettings["smtpLogin"], ConfigurationManager.AppSettings["smtpPassword"]);
        MailMessage message = new MailMessage();
        MailAddress fromAddress = new MailAddress(ConfigurationManager.AppSettings["smtpEmailFrom"]);

        public void SendEmail(string subject, string body, string to)
        {
            smtpClient.Host = ConfigurationManager.AppSettings["smtpHost"];
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = basicCredential;

            message.From = fromAddress;
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = body;
            message.To.Add(to);
            
            smtpClient.Send(message);

        }

        // metoda ktera vybere sablonu emailu a posle email vsem uzivatelum kterych se to tyka
        public void SendEmail(string template, string subject, UserModel user, DocumentModel document)
        {
            string templateString = System.IO.File.ReadAllText(System.Configuration.ConfigurationManager.AppSettings["emailTemplates"] + template + ".txt");
            templateString = templateString.Replace("[DocumentNumber]", document.DocumentNumber);
            templateString = templateString.Replace("[DocumentTitle]", document.Title);
            templateString = templateString.Replace("[UserFirstName]", user.FirstName);
            templateString = templateString.Replace("[UserLastName]", user.LastName);
            templateString = templateString.Replace("[EffeciencyDate]", document.EffeciencyDate.ToString());
            string documentLink = System.Configuration.ConfigurationManager.AppSettings["urlToServer"] + "/Documents/Details?documentId=" + document.ID.ToString();
            templateString = templateString.Replace("[DocumentLink]", documentLink);
            
            SendEmail(subject, templateString, user.Email1);
        }

    }
}
