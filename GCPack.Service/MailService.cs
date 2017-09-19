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
            // LF 12.9.2017 - pokud nechci posilat postu, hodi se dokud neni postak rozchozen
            // jedna se o volbu ve webconfigu
            // prikaz pro overeni ze se dostanes na postovni server je:
            // cmd> telnet smtp.seznam.cz 25
            if (ConfigurationManager.AppSettings["emailDisable"]!="Yes")
            {
                smtpClient.Host = ConfigurationManager.AppSettings["smtpHost"];
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = basicCredential;
                message.To.Clear();
                message.From = fromAddress;
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = body;
                message.To.Add(to);

                smtpClient.Send(message);
            }
            

        }

        // metoda ktera vybere sablonu emailu a posle email vsem uzivatelum kterych se to tyka
        public void SendEmail(string template, string subject, UserModel user, DocumentModel document)
        {
            // LF 12.9.2017 - pokud nechci posilat postu, hodi se dokud neni postak rozchozen
            // jedna se o volbu ve webconfigu
            if (ConfigurationManager.AppSettings["emailDisable"] != "Yes")
            {

                string templateString = System.IO.File.ReadAllText(System.Configuration.ConfigurationManager.AppSettings["emailTemplates"] + template + ".txt");
                templateString = templateString.Replace("[DocumentNumber]", document.DocumentNumber);
                templateString = templateString.Replace("[DocumentTitle]", document.Title);
                templateString = templateString.Replace("[UserFirstName]", user.FirstName);
                templateString = templateString.Replace("[UserLastName]", user.LastName);
                templateString = templateString.Replace("[EffeciencyDate]", document.EffeciencyDate.ToString());
                templateString = templateString.Replace("[NextRevision]", document.NextReviewDate.ToString());
                templateString = templateString.Replace("[EndDate]", document.EndDate.ToString());
                templateString = templateString.Replace("[Annotation]", (document.Annotation != null && document.Annotation.Length > 0) ? $" ANOTACE:<br/>{document.Annotation}" : string.Empty);
                string documentLink = System.Configuration.ConfigurationManager.AppSettings["urlToServer"] + "/Documents/Details?documentId=" + document.ID.ToString();
                templateString = templateString.Replace("[DocumentLink]", documentLink);

                SendEmail(subject, templateString, user.Email1);
            }
            
        }

    }
}
