using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCPack.Model;
using AutoMapper;

namespace GCPack.Repository
{
    public class DocumentsRepository : IDocumentsRepository
    {
        public DocumentModel AddDocumet(DocumentModel document)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                var newDocument = db.Documents.Add(Mapper.Map<Document>(document));
                db.SaveChanges();
                document.DocumentID = newDocument.ID;
                return document;
            }
        }

        // vrati se seznam vsech uzivatelu kteri se mohou odstranit z dokumentu
        public ICollection<User> GetDeletedUsersFromDocument(ICollection<User> users, DocumentModel document)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                ICollection<int> dbUsers = db.ReadConfirmations.Where(rc => rc.DocumentID == document.DocumentID && rc.ReadDate == null).Select(rc => rc.UserID).ToList();
                ICollection<User> deleteUsers = new HashSet<User>();
                foreach (int dbUserID in dbUsers)
                {
                    if (!users.Select(u => u.ID).Contains(dbUserID))
                        deleteUsers.Add(
                            db.Users.Where(u => u.ID == dbUserID).Select(u => u).FirstOrDefault()
                        );
                }

                return deleteUsers;

            }
        }

        // vrati se seznam vsech uzivatelu kteri se pridaji do dokumentu
        public ICollection<User> GetAddedUsersToDocument(ICollection<User> users, DocumentModel document)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                ICollection<int> dbUsers = db.ReadConfirmations.Where(rc => rc.DocumentID == document.DocumentID).Select(rc => rc.UserID).ToList();
                ICollection<User> addUsers = new HashSet<User>();
                foreach (int userID in users.Select(u => u.ID))
                {
                    if (!dbUsers.Contains(userID))
                        addUsers.Add(
                            db.Users.Where(u => u.ID == userID).Select(u => u).FirstOrDefault()
                        );
                }

                return addUsers;

            }
        }


        // pridani uzivatelu 
        public void MapUsersToDocument(ICollection<User> addUsers, DocumentModel document, ICollection<User> deleteUsers)
        {
            using (GCPackContainer db = new GCPackContainer())
            {

                db.ReadConfirmations.RemoveRange(db.ReadConfirmations.Where (rc => deleteUsers.Select(du => du.ID).Contains(rc.ID)));
                db.SaveChanges();

                foreach (User user in addUsers)
                {
                    db.ReadConfirmations.Add(new ReadConfirmation() {
                        DocumentID = document.DocumentID,
                        UserID = user.ID
                    });
                    db.SaveChanges();
                }


            }
        }

        
        // smazani dokumentu
        public void DeleteDocument(Document document)
        {

        }

        // ulozeni editovaneho dokumentu
        public Document EditDocument(Document document)
        {

            return document;
        }

        // prace se souborama prirazenyma k dokumentu
        public void AddFileToDb(Document document, string filePath)
        {

        }

        public void DeleteFileFromDocument(Document document, string filePath)
        {

        }


        }
}
