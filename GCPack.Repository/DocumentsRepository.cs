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

        public DocumentTypeModel GetDocumentType(int ID)
        {
            using (GCPackContainer db = new GCPackContainer()) {
                return Mapper.Map<DocumentTypeModel>(db.DocumentTypes.Where(dt => dt.ID == ID).Select(dt => dt).FirstOrDefault());
            }
        }

        public bool ReadAccessToDocument(DocumentModel document, int userID)
        {
            return true;
        }

        public ICollection<FileItem> GetFiles(int documentId)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                return Mapper.Map<ICollection<FileItem>>(db.Files.Where(d => d.DocumentID == documentId).Select(d => d));
            }
        }

        public DocumentTypeModel SaveDocumentType(DocumentTypeModel documentType)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                if (documentType.ID == 0)
                {
                    var dbDocumentType = db.DocumentTypes.Add(Mapper.Map<DocumentType>(documentType));
                    db.SaveChanges();
                    documentType.ID = dbDocumentType.ID;
                }
                else
                {
                    var dbDocumentType = db.DocumentTypes.Where(dt => dt.ID == documentType.ID).FirstOrDefault();
                    Mapper.Map(documentType, dbDocumentType);
                    db.SaveChanges();
                }
            }
            return documentType;
        }

        public ICollection<UserModel> GetUsersForDocument(int documentId)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                return Mapper.Map<ICollection<UserModel>>(
                    from u in db.Users
                    from rc in db.ReadConfirmations
                    where u.ID == rc.UserID && rc.DocumentID == documentId
                    select u
                    );
            }

        }

        public DocumentModel GetDocument(int documentId, int userID)
        {
            DocumentFilter filter = new DocumentFilter() { DocumentID = documentId, ForUserID = userID };
            return GetDocuments(filter).FirstOrDefault();
        }

        public ICollection<Item> GetDocumentTypes()
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                return Mapper.Map<ICollection<Item>>(db.DocumentTypes.Select(dt => dt).OrderBy(dt => dt.OrderBy));
            }
        }

            public ICollection<DocumentModel> GetDocuments(DocumentFilter filter)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                ICollection<GetDocuments_Result> documentsResult = db.GetDocuments(filter.ForUserID, filter.DocumentID, filter.Name, filter.Number, filter.AdministratorName, filter.OrderBy, filter.DocumentTypeID, 0, 100).ToList<GetDocuments_Result>();
                ICollection<DocumentModel> docs = Mapper.Map<ICollection<DocumentModel>>(documentsResult);
                return docs;
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
                    if (users == null || !users.Select(u => u).Contains(dbUserID))
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
                if (users != null)
                {
                    foreach (int userID in users.Select(u => u))
                    {
                        if (!dbUsers.Contains(userID))
                            addUsers.Add(
                                db.Users.Where(u => u.ID == userID).Select(u => u).FirstOrDefault()
                            );
                    }
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
        public void DeleteDocument(int documentId)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                db.ReadConfirmations.RemoveRange(db.ReadConfirmations.Where(rc => rc.DocumentID == documentId));
                db.SaveChanges();
                db.Files.RemoveRange(db.Files.Where(f => f.DocumentID == documentId));
                db.SaveChanges();
                db.Documents.Remove(db.Documents.Where(d => d.ID == documentId).SingleOrDefault());
                db.SaveChanges();
            }
        }

        public FileItem GetFile(int fileID)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                var blob = db.Files.Where(f => f.ID == fileID).Select(f => f.FileBlob).FirstOrDefault();
                string fileName = db.Files.Where(f => f.ID == fileID).Select(f => f.Name).FirstOrDefault();
                return new FileItem() {Data = blob, Name = fileName };
            }
        }

        /// <summary>
        /// Overeni zda ma uzivatel pristup na cteni tohoto dokumentu
        /// </summary>
        /// <returns></returns>
        public bool ReadAccessToDocument(Document document, int userID)
        {
            bool result = false;
            using (GCPackContainer db = new GCPackContainer())
            {
                result = (db.ReadConfirmations.Where(rc => rc.DocumentID == document.ID && rc.UserID == userID).Count() > 0);
                // pokud neni uzivatel prirazen k dokumentu, pak se jeste overi
                // zda neni administratorem dokumentu
                if (!result)
                {
                    if (document.DocumentAdminType == (int)DocumentAdminType.User)
                    {
                        // pokud je dokument typu vybrana osoba
                        result = (document.AdministratorID == userID);
                    }
                    else
                    {
                        // overeni ze je osoba spravcem dokumentu pres typ dokumentu
                        result = (db.DocumentTypes.Where(dt => dt.ID == document.DocumentTypeID && dt.User.ID == userID).Count() > 0);
                    }
                }
            }

            return result;
        }

            public void Readed(int documentID, int userID)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                var confirm = db.ReadConfirmations.Where(rc => rc.DocumentID == documentID && rc.UserID == userID).FirstOrDefault();
                if (confirm != null)
                {
                    confirm.ReadDate = DateTime.Now;
                    db.SaveChanges();
                }
            }
        }

        // ulozeni editovaneho dokumentu
        public DocumentModel EditDocument(DocumentModel document)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                var dbDocument = db.Documents.Where(d => d.ID == document.ID).Select(d => d).FirstOrDefault();
                int stateID = (int)dbDocument.StateID;
                Mapper.Map(document, dbDocument);
                dbDocument.StateID = stateID;
                db.SaveChanges();
            }
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

        public void ChangeDocumentState(DocumentModel document, string state)
        {
            ChangeDocumentState(document.ID, state);
        }

        public void ChangeDocumentState(int documentId, string state)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                int stateID = db.States.Where(s => s.Code == state).Select(s => s.ID).FirstOrDefault();
                db.Documents.Where(d => d.ID == documentId).Select(d => d).FirstOrDefault().StateID = stateID;
                db.SaveChanges();
            }
        }

        public void DeleteFilesFromDocument(DocumentModel document)
        {
            if (document.DeleteFileItems != null)
            {
                string[] deletedStringFiles = document.DeleteFileItems.Split(',').Where(d => d != "").Select(d => d).ToArray();
                int[] deletedFiles = Array.ConvertAll(deletedStringFiles, int.Parse);
                using (GCPackContainer db = new GCPackContainer())
                {
                    db.Files.RemoveRange(db.Files.Where(f => deletedFiles.Contains(f.ID)));
                    db.SaveChanges();
                }
            }
        }


        }
}
