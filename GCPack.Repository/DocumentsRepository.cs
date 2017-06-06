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
                var stateID = db.States.Where(s => s.Code == "New").Select(s => s.ID).FirstOrDefault();
                var documentType = db.DocumentTypes.Where(dt => dt.ID == document.DocumentTypeID).Select(dt => dt).SingleOrDefault();
                document.StateID = stateID;
                var newDocument = db.Documents.Add(Mapper.Map<Document>(document));
                newDocument.DocumentType = documentType; 
                db.SaveChanges();
                document.ID = newDocument.ID;
                return document;
            }
        }

        public ICollection<FileItem> GetFiles(int documentId)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                return Mapper.Map<ICollection<FileItem>>(db.Files.Where(d => d.DocumentID == documentId).Select(d => d));
            }
        }

        public DocumentModel GetDocument(int documentId)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                return Mapper.Map<DocumentModel>(db.Documents.Where(d => d.ID == documentId).Select(d => d).SingleOrDefault());
            }
        }

        public ICollection<DocumentModel> GetDocuments(DocumentFilter filter)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                // TODO: Dopsat filtrovaci podminky
                return Mapper.Map<ICollection<DocumentModel>>(db.Documents.Select (d => d));
            }
        }
        // vrati se seznam vsech uzivatelu kteri se mohou odstranit z dokumentu
        public ICollection<UserModel> GetDeletedUsersFromDocument(ICollection<int> users, DocumentModel document)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                ICollection<int> dbUsers = db.ReadConfirmations.Where(rc => rc.DocumentID == document.ID && rc.ReadDate == null).Select(rc => rc.UserID).ToList();
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
                ICollection<int> dbUsers = db.ReadConfirmations.Where(rc => rc.DocumentID == document.ID).Select(rc => rc.UserID).ToList();
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
                if (deleteUsers != null)
                {
                    db.ReadConfirmations.RemoveRange(
                        db.ReadConfirmations.Where(
                            rc =>
                                deleteUsers.Select(du => du).Contains(rc.UserID)
                                && rc.DocumentID == document.ID
                                )
                            );
                    db.SaveChanges();
                }

                if (addUsers != null)
                {
                    foreach (int userId in addUsers)
                    {
                        db.ReadConfirmations.Add(new ReadConfirmation()
                        {
                            DocumentID = document.ID,
                            UserID = userId
                        });
                        db.SaveChanges();
                    }
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
        public void AddFileToDb(DocumentModel document, byte[] fileStream, string name)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                db.Files.Add(new File() {
                    Name = name,
                    FileBlob = fileStream,
                    Document = db.Documents.Where (d => d.ID == document.ID).Select(d => d).FirstOrDefault()
                });
                db.SaveChanges();
            }
        }

        public void DeleteFileFromDocument(DocumentModel document, string fileName)
        {

        }


        }
}
