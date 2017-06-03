using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCPack.Model;
using AutoMapper;
using GCPack.Repository.Interfaces;

namespace GCPack.Repository
{
    public class DocumentsRepository : IDocumentsRepository
    {
        public DocumentModel AddDocument(DocumentModel document)
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
        public ICollection<UserModel> GetDeletedUsersFromDocument(ICollection<int> users, DocumentModel document)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                ICollection<int> dbUsers = db.ReadConfirmations.Where(rc => rc.DocumentID == document.DocumentID && rc.ReadDate == null).Select(rc => rc.UserID).ToList();
                ICollection<User> deleteUsers = new HashSet<User>();

                foreach (int dbUserID in dbUsers)
                {
                    if (!users.Select(u => u).Contains(dbUserID))
                        deleteUsers.Add(
                            db.Users.Where(u => u.ID == dbUserID).Select(u => u).FirstOrDefault()
                        );
                }

                return Mapper.Map<ICollection<UserModel>>(deleteUsers);

            }
        }

        // vrati se seznam vsech uzivatelu kteri se pridaji do dokumentu
        public ICollection<UserModel> GetAddedUsersToDocument(ICollection<int> users, DocumentModel document)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                ICollection<int> dbUsers = db.ReadConfirmations.Where(rc => rc.DocumentID == document.DocumentID).Select(rc => rc.UserID).ToList();
                ICollection<User> addUsers = new HashSet<User>();
                foreach (int userID in users.Select(u => u))
                {
                    if (!dbUsers.Contains(userID))
                        addUsers.Add(
                            db.Users.Where(u => u.ID == userID).Select(u => u).FirstOrDefault()
                        );
                }

                return Mapper.Map<ICollection<UserModel>>(addUsers);

            }
        }


        // pridani uzivatelu 
        public void MapUsersToDocument(ICollection<int> addUsers, DocumentModel document, ICollection<int> deleteUsers)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                db.ReadConfirmations.RemoveRange(
                    db.ReadConfirmations.Where (
                        rc => 
                            deleteUsers.Select(du => du).Contains(rc.ID)
                            && rc.DocumentID == document.DocumentID
                            )  
                        );
                db.SaveChanges();

                foreach (int userId in addUsers)
                {
                    db.ReadConfirmations.Add(new ReadConfirmation() {
                        DocumentID = document.DocumentID,
                        UserID = userId
                    });
                    db.SaveChanges();
                }


            }
        }

        
        // smazani dokumentu
        public void DeleteDocument(DocumentModel document)
        {

        }

        // ulozeni editovaneho dokumentu
        public DocumentModel EditDocument(DocumentModel document)
        {

            return document;
        }

        // prace se souborama prirazenyma k dokumentu
        public void AddFileToDb(DocumentModel document, string filePath)
        {

        }

        public void DeleteFileFromDocument(DocumentModel document, string filePath)
        {

        }


        }
}
