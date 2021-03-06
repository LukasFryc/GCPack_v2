﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCPack.Model;
using AutoMapper;
using GCPack.Repository.Interfaces;
using System.Data.Entity.Core.Objects;

namespace GCPack.Repository
{
    public class DocumentsRepository : IDocumentsRepository
    {
        // vraceni id stavu dokumentu z jeho kodu - kvuli prehlednosti se posila kod a vraci se ID

        public int GetDocumentState(string state)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                return db.DocumentStates.Where(s => s.Code == state).Select(s => s.ID).FirstOrDefault();
            }
        }

        // metoda ktera vraci vsechny prirazene uzivatele a vsechny uzivatele kteri
        // se s dokumentem seznamili

        public UsersInDocument GetUsersInDocument(int documentID)
        {
            UsersInDocument usersInDocument = new UsersInDocument();
            using (GCPackContainer db = new GCPackContainer())
            {
                var allUsers1 = from j in db.JobPositionDocuments
                                from u in db.JobPositionUsers
                                where j.JobPositionId == u.JobPositionId && j.DocumentId == documentID
                                select u.User;
                var allUsers2 = from ud in db.UserDocuments
                                where ud.DocumentId == documentID
                                select ud.User;


                var allUsers = allUsers1.Union(allUsers2);



                var usersRead = from r in db.ReadConfirmations
                                from u in db.Users
                                where
                                    r.DocumentID == documentID &&
                                    u.ID == r.UserID
                                select new { User = u, Date = r.ReadDate };
                usersInDocument.DocumentID = documentID;

                usersInDocument.AllUsers = Mapper.Map<ICollection<UserModel>>(allUsers);
                foreach (var userRead in usersRead)
                {
                    usersInDocument.UsersRead.Add(new UserReadDocument()
                    {
                        //(DateTime)
                        DateRead = userRead.Date,
                        User = Mapper.Map<UserModel>(userRead.User)
                    });
                }
                return usersInDocument;
            }

        }



        public void SetNumberOfDocument(int documentTypeID)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                var documentType = db.DocumentTypes.Where(dt => dt.ID == documentTypeID).Select(dt => dt).FirstOrDefault();
                documentType.LastNumberOfDocument += 1;
                db.SaveChanges();
            }
        }



        // priklad paging 
        public ICollection<DocumentModel> GetDocuments_priklad(DocumentFilter filter)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                var q = db.Documents.Where(d => d.EffeciencyDate == DateTime.Now).OrderBy(d => d.EffeciencyDate).Skip(filter.Page * filter.ItemsPerPage).Take(filter.ItemsPerPage).Select(d => d);
                return Mapper.Map<ICollection<DocumentModel>>(q);
            }

        }
        public DocumentTypeModel GetDocumentType(int ID)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
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
                    dbDocumentType.AuthorizinOfficerID = 0;
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
                    from rc in db.UserDocuments
                    where u.ID == rc.UserId && rc.DocumentId == documentId
                    select u
                    );
            }

        }

        public void SaveListCodes(DocumentModel document)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                db.DivisionDocuments.RemoveRange(db.DivisionDocuments.Where(dd => dd.DocumentID == document.ID));
                db.SystemDocuments.RemoveRange(db.SystemDocuments.Where(sd => sd.DocumentID == document.ID));
                db.ProjectDocuments.RemoveRange(db.ProjectDocuments.Where(pd => pd.DocumentID == document.ID));
                db.WorkplaceDocuments.RemoveRange(db.WorkplaceDocuments.Where(pd => pd.DocumentID == document.ID));
                db.SaveChanges();

                if (document.SelectedDivisionsID != null)
                {
                    foreach (int divisionID in document.SelectedDivisionsID)
                    {
                        db.DivisionDocuments.Add(new DivisionDocument() { DivisionID = divisionID, DocumentID = document.ID });
                    }
                }

                if (document.SelectedAppSystemsID != null)
                {
                    foreach (int systemID in document.SelectedAppSystemsID)
                    {
                        db.SystemDocuments.Add(new SystemDocument() { ID_System = systemID, DocumentID = document.ID });
                    }
                }

                if (document.SelectedProjectsID != null)
                {
                    foreach (int projectID in document.SelectedProjectsID)
                    {
                        db.ProjectDocuments.Add(new ProjectDocument() { ProjectID = projectID, DocumentID = document.ID });
                    }
                }

                if (document.SelectedWorkplacesID != null)
                {
                    foreach (int workplaceID in document.SelectedWorkplacesID)
                    {
                        db.WorkplaceDocuments.Add(new WorkplaceDocument() { WorkplaceID = workplaceID, DocumentID = document.ID });
                    }
                }

                db.SaveChanges();

            }
        }

        public ICollection<int> GetAppSystemsFromDocument(int documentId)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                ICollection<int> AppSystemsID = db.Documents.Where(d => d.ID == documentId).Select(d => d).FirstOrDefault().SystemDocuments.Select(sd => sd.ID_System).ToList<int>();
                return AppSystemsID;
            }
        }

        public ICollection<int> GetAppProjectsFromDocument(int documentId)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                ICollection<int> ProjectsID = db.Documents.Where(d => d.ID == documentId).Select(d => d).FirstOrDefault().ProjectDocuments.Select(sd => sd.ProjectID).ToList<int>();
                return ProjectsID;
            }
        }

        public ICollection<int> GetAppDivisionsFromDocument(int documentId)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                ICollection<int> DivisionsID = db.Documents.Where(d => d.ID == documentId).Select(d => d).FirstOrDefault().DivisionDocuments.Select(sd => sd.DivisionID).ToList<int>();
                return DivisionsID;
            }
        }



        public DocumentModel GetDocument(int documentId, int? userID)
        {
            // LF 
            // UserId - se zde prebira pouze  u edit a detail dokumentu a to v contoleru, prebira se y aktualne prihlasene osoby
            // timto se yajisti i napr to, ye uyivatel s roli User se nedostane zadnym zpusobem na dokumenty 
            // kter jsou v jinem stavu nez Registered a jsou typ revize = p
            // nejde to obejit parametrz z prohlizece

            DocumentFilter filter = new DocumentFilter() { DocumentID = documentId, ForUserID = userID };
            DocumentModel document = GetDocuments(filter).Documents.FirstOrDefault();
            return document;
        }

        public ICollection<Item> GetDocumentTypes()
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                return Mapper.Map<ICollection<Item>>(db.DocumentTypes.Select(dt => dt).OrderBy(dt => dt.OrderBy));
            }
        }
       

        public DocumentCollectionModel GetDocuments_linq(DocumentFilter filter)
        {
            DocumentCollectionModel documentCollection = new DocumentCollectionModel();
            using (GCPackContainer db = new GCPackContainer())
            {
                filter.ReadType = (filter.ReadType is null) ? "all" : filter.ReadType;

                if (!string.IsNullOrEmpty(filter.StateCode)) filter.StateID = GetDocumentState(filter.StateCode); // lF 25.10.2017

                if (filter.DocumentID == null) filter.DocumentID = 0;
                
                var documents = (
                    from d in db.Documents
                    select d);

                // generovani where podminek
                
                if (filter.DocumentID != 0) documents = documents.Where(d => d.ID == filter.DocumentID);
                if (filter.StateID != 0 && filter.StateID != null) documents = documents.Where(d => d.StateID == filter.StateID);
                if (filter.Revision != "all" && filter.Revision != null) documents = documents.Where(d => d.Revision == filter.Revision);
                if (filter.ReviewNecessaryChange != "all" && filter.ReviewNecessaryChange != null) documents = documents.Where(d => d.ReviewNecessaryChange == true);

                if (!string.IsNullOrEmpty(filter.Name)) documents = documents.Where(d => d.Title.Contains(filter.Name));
                if (!string.IsNullOrEmpty(filter.Number)) documents = documents.Where(d => d.DocumentNumber.Contains(filter.Number));
                if (filter.DocumentTypeID != 0 && filter.DocumentTypeID != null) documents = documents.Where(d => d.DocumentTypeID == filter.DocumentTypeID);

                if (filter.ProjectID != 0 && filter.ProjectID != null)
                    documents = documents.Where(
                        d =>
                            db.ProjectDocuments.Where(pd => pd.ProjectID == filter.ProjectID).Select(pd => pd.DocumentID)
                            .Contains(d.ID)
                            );

                if (filter.DivisionID != 0 && filter.DivisionID != null)
                    documents = documents.Where(
                        d =>
                            db.DivisionDocuments.Where(dd => dd.DivisionID == filter.DivisionID).Select(dd => dd.DocumentID)
                            .Contains(d.ID)
                            );
                if (filter.AppSystemID != 0 && filter.AppSystemID != null)
                    documents = documents.Where(
                        d =>
                            db.SystemDocuments.Where(sd => sd.ID_System == filter.AppSystemID).Select(sd => sd.DocumentID)
                            .Contains(d.ID)
                            );

                if (filter.WorkplaceID != 0 && filter.WorkplaceID != null)
                    documents = documents.Where(
                        d =>
                            db.WorkplaceDocuments.Where(wp => wp.WorkplaceID == filter.WorkplaceID).Select(wp => wp.DocumentID)
                            .Contains(d.ID)
                            );

                if (filter.NextReviewDateFrom != null)
                    documents = documents.Where(d => filter.NextReviewDateFrom <= d.NextReviewDate);

                if (filter.NextReviewDateTo != null)
                {
                    filter.NextReviewDateTo = filter.NextReviewDateTo.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                    documents = documents.Where(d => filter.NextReviewDateTo >= d.NextReviewDate);
                }

                if (filter.EffeciencyDateFrom != null)
                    documents = documents.Where(d => filter.EffeciencyDateFrom <= d.EffeciencyDate);

                if (filter.EffeciencyDateTo != null)
                {
                    filter.EffeciencyDateTo = filter.EffeciencyDateTo.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                    documents = documents.Where(d => filter.EffeciencyDateTo >= d.EffeciencyDate);
                }


                if (filter.ReadType == "allWith")
                {
                    documents = documents.Where(d => d.ReadConfirmations.Where(rc => rc.UserID == filter.ForUserID).Select(rc => rc.DocumentID).Contains(d.ID));
                }


                if (filter.ReadType == "read")
                {
                    documents = documents.Where(d => d.ReadConfirmations.Where(rc => rc.ReadDate.Equals(null) && rc.UserID == filter.ForUserID).Select(rc => rc.DocumentID).Contains(d.ID));
                }

                if (filter.ReadType == "unread")
                {
                    documents = documents.Where(d => d.ReadConfirmations.Where(rc => rc.ReadDate.Equals(null) && rc.UserID == filter.ForUserID).Select(rc => rc.DocumentID).Contains(d.ID));
                }

                if (!string.IsNullOrEmpty(filter.AdministratorName))
                {
                    documents =
                      documents.Where(d =>
                       //(d.AdministratorID == 0 && 
                       //    (
                       //        d.DocumentType.User.FirstName.Contains (filter.AdministratorName) ||
                       //        d.DocumentType.User.LastName.Contains(filter.AdministratorName)
                       //    )) ||
                       (d.AdministratorID != 0 &&
                          (
                              db.Users.Where(u => u.FirstName.Contains(filter.AdministratorName) ||
                             u.LastName.Contains(filter.AdministratorName)).Select(u => u.ID).Contains(d.AdministratorID)
                          )
                       )
                      );
                }
                
                switch (filter.OrderBy)
                {
                    case "AuthorA":
                        documents = documents.OrderBy(d => d.User1.LastName).ThenBy (d => d.User1.FirstName);
                        break;
                    case "AuthorD":
                        documents = documents.OrderByDescending(d => d.User1.LastName).ThenByDescending(d => d.User1.FirstName);
                        break;
                    case "AdminA":
                        documents = documents.OrderBy(d => d.User.LastName).ThenBy(d => d.User.FirstName);
                        break;
                    case "AdminD":
                        documents = documents.OrderByDescending(d => d.User.LastName).ThenByDescending(d => d.User.FirstName);
                        break;
                    case "NameA":
                        documents = documents.OrderBy(d => d.Title);
                        break;
                    case "NameD":
                        documents = documents.OrderByDescending(d => d.Title);
                        break;
                    case "NumberA":
                        documents = documents.OrderBy(d => d.DocumentNumber).ThenBy(d => d.IssueNumber);
                        break;
                    case "NumberD":
                        documents = documents.OrderByDescending(d => d.DocumentNumber).ThenBy(d => d.IssueNumber);
                        break;
                    case "RevisionA":
                        documents = documents.OrderBy(d => d.EffeciencyDate);
                        break;
                    case "RevisionD":
                        documents = documents.OrderByDescending(d => d.EffeciencyDate);
                        break;
                    default:
                        documents = documents.OrderBy(d => d.DocumentNumber).ThenBy(d => d.IssueNumber);
                        break;
                }

                // tyka se strankovani: documents.Skip(filter.Page * filter.ItemPerPage).Take (filter.ItemPerPage)

                documentCollection.Count = documents.Count();

                int page = (filter.Page != 0) ? filter.Page - 1 : filter.Page;
                int itemsPerPage = (filter.ItemsPerPage == 0) ? 1 : filter.ItemsPerPage;
                
                var items = documents.Skip(page * itemsPerPage).Take(itemsPerPage).Select(
                    d => new  { d,
                        AllUsers = ((from rc in db.ReadConfirmations where rc.DocumentID == d.ID select rc).Count()),
                        UsersRead = ((from rc in db.ReadConfirmations where rc.DocumentID == d.ID && !rc.ReadDate.Equals(null) select rc).Count()),
                        CanConfirmRead = ((from rc in db.ReadConfirmations where rc.DocumentID == d.ID && rc.UserID == filter.ForUserID && rc.ReadDate.Equals(null) select rc).Count() > 0 ),
                        DocumentAdministrator = ((from u in db.Users where u.ID == d.AdministratorID select u.LastName + " " + u.FirstName).FirstOrDefault()),
                        AuthorName = ((from u in db.Users where u.ID == d.AuthorID select u.LastName + " " + u.FirstName).FirstOrDefault()),
                        DocumentStateName = ((from s in db.DocumentStates where s.ID == d.StateID select s.Name).FirstOrDefault()),
                        DocumentStateCode = ((from s in db.DocumentStates where s.ID == d.StateID select s.Code).FirstOrDefault())
                    }
                    );

                

                ICollection <DocumentModel> documentsResult = new HashSet<DocumentModel>();
                foreach (var item in items)
                {
                    DocumentModel documentModelResult = Mapper.Map<DocumentModel>(item.d);
                    documentModelResult.AllUsers = item.AllUsers;
                    documentModelResult.UsersRead = item.UsersRead;
                    documentModelResult.CanConfirmRead = item.CanConfirmRead;
                    documentModelResult.DocumentAdministrator = item.DocumentAdministrator;
                    documentModelResult.AuthorName = item.AuthorName;
                    documentModelResult.DocumentStateName = item.DocumentStateName;
                    documentModelResult.DocumentStateCode = item.DocumentStateCode;


                    documentsResult.Add(documentModelResult);
                }

                documentCollection.Documents = documentsResult;
                documentCollection.filter = filter;



                return documentCollection;
            }
        }


        public DocumentCollectionModel GetDocuments(DocumentFilter filter)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                DocumentCollectionModel documentCollection = new DocumentCollectionModel();
                filter.ReadType = (filter.ReadType is null) ? "all" : filter.ReadType;

                //LF 11.12.2017 NOVE
                if (filter.UserID == null || filter.UserID == 0)
                {
                    filter.UserID = filter.ForUserID;
                }

                if (!string.IsNullOrEmpty(filter.StateCode)) filter.StateID = GetDocumentState(filter.StateCode); // lF 25.10.2017

                if (filter.DocumentID == null) filter.DocumentID = 0;

                //LF 11.12.2017 NOVE
                //AdministratorID: $('#AdministratorID').val(),
                        //AuthorID: $('#AuthorID').val(),

                ICollection<GetDocuments23_Result> documentsResult = db.GetDocuments23(filter.UserID, filter.DocumentID, filter.Name, filter.Number, filter.AdministratorName, filter.OrderBy, filter.DocumentTypeID, 0, 100000, filter.ProjectID, filter.DivisionID, filter.AppSystemID, filter.WorkplaceID, filter.NextReviewDateFrom, filter.NextReviewDateTo, filter.EffeciencyDateFrom, filter.EffeciencyDateTo, filter.ReadType, filter.StateID, filter.Revision, filter.ReviewNecessaryChange, filter.MainID, filter.AdministratorID, filter.AuthorID).ToList<GetDocuments23_Result>();
                //ICollection<GetDocuments22_Result> documentsResult = db.GetDocuments22(filter.ForUserID, filter.DocumentID, filter.Name, filter.Number, filter.AdministratorName, filter.OrderBy, filter.DocumentTypeID, 0, 100000, filter.ProjectID, filter.DivisionID, filter.AppSystemID, filter.WorkplaceID, filter.NextReviewDateFrom, filter.NextReviewDateTo, filter.EffeciencyDateFrom, filter.EffeciencyDateTo, filter.ReadType, filter.StateID, filter.Revision, filter.ReviewNecessaryChange, filter.MainID).ToList<GetDocuments22_Result>();
                // prdchozi radek puvodne do 11.12.2017

                documentCollection.Count = documentsResult.Count();
                // v pripade ze se jedna o vyber jednoho dokumentu
                if (filter.DocumentID != 0)
                {
                    filter.ItemsPerPage = 1;
                    filter.Page = 1;
                }

                documentCollection.Documents = Mapper.Map<ICollection<DocumentModel>>(documentsResult.Skip((filter.Page - 1) * filter.ItemsPerPage).Take(filter.ItemsPerPage));

                // LF 4.12.2017 
                documentCollection.filter = filter;

                return documentCollection;
            }
        }
        // vrati se seznam vsech uzivatelu kteri se mohou odstranit z dokumentu
        public ICollection<UserModel> GetDeletedUsersFromDocument(ICollection<int> users, DocumentModel document)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                ICollection<int> dbUsers = db.UserDocuments.Where(rc => rc.DocumentId == document.ID).Select(rc => rc.UserId).ToList();
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

        // seznam vsech pracovnich pozic (funkci) pridelenych k dokumentu
        public ICollection<int> GetJobPositionsFromDocument(int documentId)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                return db.JobPositionDocuments.Where(jpd => jpd.DocumentId == documentId).Select(jpd => jpd.JobPositionId).ToList();
            }
        }


        // vrati se seznam vsech uzivatelu kteri se pridaji do dokumentu
        public ICollection<UserModel> GetAddedUsersToDocument(ICollection<int> users, DocumentModel document)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                ICollection<int> dbUsers = db.UserDocuments.Where(rc => rc.DocumentId == document.ID).Select(rc => rc.UserId).ToList();
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
                    db.UserDocuments.RemoveRange(
                        db.UserDocuments.Where(
                            rc =>
                                deleteUsers.Select(du => du).Contains(rc.UserId)
                                && rc.DocumentId == document.ID
                                )
                            );
                    db.SaveChanges();
                }

                if (addUsers != null)
                {
                    foreach (int userId in addUsers)
                    {
                        db.UserDocuments.Add(new UserDocument()
                        {
                            DocumentId = document.ID,
                            UserId = userId,
                            Created = DateTime.Now
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

                db.UserDocuments.RemoveRange(db.UserDocuments.Where(rc => rc.DocumentId == documentId));
                db.SaveChanges();

                db.Files.RemoveRange(db.Files.Where(f => f.DocumentID == documentId));
                db.SaveChanges();

                db.JobPositionDocuments.RemoveRange(db.JobPositionDocuments.Where(jpd => jpd.DocumentId == documentId));
                db.SaveChanges();

                db.DivisionDocuments.RemoveRange(db.DivisionDocuments.Where(jpd => jpd.DocumentID == documentId));
                db.SaveChanges();

                db.ProjectDocuments.RemoveRange(db.ProjectDocuments.Where(jpd => jpd.DocumentID == documentId));
                db.SaveChanges();

                db.SystemDocuments.RemoveRange(db.SystemDocuments.Where(jpd => jpd.DocumentID == documentId));
                db.SaveChanges();

                db.WorkplaceDocuments.RemoveRange(db.WorkplaceDocuments.Where(jpd => jpd.DocumentID == documentId));
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
                return new FileItem() { Data = blob, Name = fileName};
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
                // TODO: upravit na overeni uzivatele dokumentu oproti jobpositions
                result = (db.UserDocuments.Where(rc => rc.DocumentId == document.ID && rc.UserId == userID).Count() > 0);
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
                        result = (db.DocumentTypes.Where(dt => dt.ID == document.DocumentTypeID && dt.AdministratorID == userID).Count() > 0);
                    }
                }
            }

            return result;
        }

        public void Readed(int documentID, int userID)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                //ReadConfirmation readConfirmation = null;
                var readConfirms = db.ReadConfirmations.Where(s => s.DocumentID == documentID && s.UserID == userID).Select(s => s);
                if (readConfirms != null)
                {

                    foreach (var item in readConfirms)
                    {
                        item.ReadDate = DateTime.Now;

                    }

                    //readConfirmation = db.ReadConfirmations.Add(new ReadConfirmation()
                    //{
                    //    DocumentID = documentID,
                    //    UserID = userID,
                    //    ReadDate = DateTime.Now
                    //});
                    db.SaveChanges();
                }
                //return readConfirmation.ID;
            }
        }

        //public void ReadedUserInFunctions(int confirmID, int jobPositionID, string name)
        //{
        //    using (GCPackContainer db = new GCPackContainer())
        //    {
        //        {
        //            db.ConfirmUserFunctions.Add(new ConfirmUserFunction()
        //            {
        //                ConfimID = confirmID,
        //                JobPositionID = jobPositionID,
        //                JobPositionName = name
        //            });
        //            db.SaveChanges();
        //        }

        //    }
        //}


        public DocumentModel AddDocument(DocumentModel document)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                document.Title = (document.Title == null) ? string.Empty : document.Title;
                document.DocumentNumber = (document.DocumentNumber == null) ? string.Empty : document.DocumentNumber;

                var stateID = db.DocumentStates.Where(s => s.Code == "New").Select(s => s.ID).FirstOrDefault();
                var documentType = db.DocumentTypes.Where(dt => dt.ID == document.DocumentTypeID).Select(dt => dt).SingleOrDefault();
                document.StateID = stateID;
                var newDocument = db.Documents.Add(Mapper.Map<Document>(document));
                newDocument.DocumentTypeID = documentType.ID;
                db.SaveChanges();
                document.ID = newDocument.ID;
                document.MainID = newDocument.ID;

                // pokud se jedna o novy dokument (a ne nove vydani), pak se parentID nastavi na ID dokumentu
                if (newDocument.ParentID == 0)
                {
                    //newDocument.ParentID = document.ID;
                    newDocument.MainID = document.ID;

                    db.SaveChanges();
                }


                if (document.JobPositionIDs != null)
                {
                    foreach (int jobPositionID in document.JobPositionIDs)
                    {
                        db.JobPositionDocuments.Add(new JobPositionDocument() { DocumentId = document.ID, JobPositionId = jobPositionID, Created = DateTime.Now });
                    }
                    db.SaveChanges();
                }
                //return Mapper.Map<DocumentModel>(newDocument);

                return document;
            }
        }

        public void ChangeRevison(DocumentModel document, string revisionType)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                var dbDocument = db.Documents.Where(d => d.ID == document.ID).Select(d => d).FirstOrDefault();
                dbDocument.Revision = revisionType;
                db.SaveChanges();
            }
        }
        // ulozeni editovaneho dokumentu
        public DocumentModel EditDocument(DocumentModel document)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                var dbDocument = db.Documents.Where(d => d.ID == document.ID).Select(d => d).FirstOrDefault();
                //int stateID = (int)dbDocument.StateID;
                document.DocumentTypeID = dbDocument.DocumentTypeID;
                document.ParentID = (int)dbDocument.ParentID;
                // pokud je jiz vygenerovane cislo dokumentu, tak se vezme z databaze - jinak se necha stavajici
                document.DocumentNumber = (string.IsNullOrEmpty(document.DocumentNumber)) ? dbDocument.DocumentNumber : document.DocumentNumber;
                Mapper.Map(document, dbDocument);
                //dbDocument.StateID = stateID;
                db.SaveChanges();

                db.JobPositionDocuments.RemoveRange(db.JobPositionDocuments.Where(jpd => jpd.DocumentId == document.ID));
                db.SaveChanges();

                if (document.JobPositionIDs != null)
                {
                    foreach (int jobPositionID in document.JobPositionIDs)
                    {
                        db.JobPositionDocuments.Add(new JobPositionDocument() { DocumentId = document.ID, JobPositionId = jobPositionID, Created = DateTime.Now });
                    }
                    db.SaveChanges();
                }
            }
            return document;
        }

        // prace se souborama prirazenyma k dokumentu
        public void AddFileToDb(DocumentModel document, byte[] fileStream, string name)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                db.Files.Add(new File()
                {
                    Name = name,
                    FileBlob = fileStream,
                    Document = db.Documents.Where(d => d.ID == document.ID).Select(d => d).FirstOrDefault()
                });
                db.SaveChanges();
            }
        }
        public void ChangeDocumentState(DocumentModel document, string state)
        {

            ChangeDocumentState(document.ID, state);

        }

        public void ChangeDocumentState(DocumentModel document, string state, string helpText)
        {
            if (String.IsNullOrEmpty(helpText))
            {
                ChangeDocumentState(document.ID, state);
            }
            else
            {
                ChangeDocumentState(document.ID, state, helpText);
            }
        }

        public void ChangeDocumentState(int documentId, string state)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                int stateID = db.DocumentStates.Where(s => s.Code == state).Select(s => s.ID).FirstOrDefault();
                var dbDocument = db.Documents.Where(d => d.ID == documentId).Select(d => d).FirstOrDefault();

                dbDocument.PreviousStateID = dbDocument.StateID;
                dbDocument.StateID = stateID;

                db.SaveChanges();
            }
        }

        public void ChangeDocumentState(int documentId, string state, string helpText)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                int stateID = db.DocumentStates.Where(s => s.Code == state).Select(s => s.ID).FirstOrDefault();
                var dbDocument = db.Documents.Where(d => d.ID == documentId).Select(d => d).FirstOrDefault();

                dbDocument.PreviousStateID = dbDocument.StateID;
                dbDocument.StateID = stateID;

                if (state == "Storno")
                {
                    dbDocument.StornoReason = helpText;
                }




                db.SaveChanges();
            }
        }

        //public void Archived(DocumentModel document, bool archiv)
        //{
        //    Archived(document.ID, archiv);
        //}

        //public void Archived(int documentId, bool archiv)
        //{
        //    using (GCPackContainer db = new GCPackContainer())
        //    {
        //        db.Documents.Where(d => d.ID == documentId).Select(d => d).FirstOrDefault().Archived = archiv;
        //        db.SaveChanges();
        //    }
        //}

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

        public string GenNumberOfDocument(int documentTypeID)
        {
            string numberOfDocument = "";
            using (GCPackContainer db = new GCPackContainer())
            {
                var documentType = db.DocumentTypes.Where(dt => dt.ID == documentTypeID).Select(dt => dt).FirstOrDefault();
                numberOfDocument = documentType.NumberingOfDocumentPrefix + documentType.NumberingOfDocumentSeparator + documentType.LastNumberOfDocument.ToString().PadLeft(System.Convert.ToInt32(documentType.NumberingOfDocumentLength), '0');
            }
            return numberOfDocument;
        }

        public ICollection<int> GetWorkplacesFromDocument(int documentId)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                ICollection<int> WorkplacesID = db.Documents.Where(d => d.ID == documentId).Select(d => d).FirstOrDefault().WorkplaceDocuments.Select(sd => sd.WorkplaceID).ToList<int>();
                return WorkplacesID;
            }
        }

        public void ReviewNoAction(DocumentModel document)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                var dbDocument = db.Documents.Where(d => d.ID == document.ID).Select(d => d).FirstOrDefault();
                dbDocument.ReviewDate = DateTime.Now;
                DocumentTypeModel documentTypeModel = GetDocumentType(dbDocument.DocumentTypeID);
                dbDocument.NextReviewDate = DateTime.Now.AddYears(documentTypeModel.ValidityInYears);
                dbDocument.ReviewNecessaryChange = false;

                db.SaveChanges();
            }


        }

        public void ReviewNecessaryChange(DocumentModel document, string comment, string userName)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                var dbDocument = db.Documents.Where(d => d.ID == document.ID).Select(d => d).FirstOrDefault();

                StringBuilder builder = new StringBuilder(dbDocument.ReviewNecessaryChangeComment);
                builder.AppendLine(DateTime.Now.ToString() + " - " + userName + " :");
                builder.AppendLine(comment);
                dbDocument.ReviewNecessaryChangeComment = builder.ToString();
                dbDocument.ReviewNecessaryChange = true;

                db.SaveChanges();
            }


        }

        public void ChangeDocumentStateOnPreviousState(DocumentModel document, string newState)
        {
            using (GCPackContainer db = new GCPackContainer())
            {

                int? stateID;
                var dbDocument = db.Documents.Where(d => d.ID == document.ID).Select(d => d).FirstOrDefault();
                stateID = dbDocument.PreviousStateID;
                if (newState != "")
                {
                    stateID = db.DocumentStates.Where(s => s.Code == newState).Select(s => s.ID).FirstOrDefault();
                }

                if (stateID != null)
                {
                    dbDocument.PreviousStateID = dbDocument.StateID;
                    dbDocument.StateID = stateID;
                    db.SaveChanges();
                }
            }
        }


        //public ICollection<UsersForJobPositionInDocumentModel> GetUsersForJobPositionInDocument(int documentId, ICollection<int> jobPositionsID, ICollection<int> usersID)
        //{

        //    ICollection<UsersForJobPositionInDocumentModel> jpu = new HashSet<UsersForJobPositionInDocumentModel>();
        //    using (GCPackContainer db = new GCPackContainer())
        //    {
        //        var result = db.JobPositions
        //            .Where(jp => jobPositionsID.Contains(jp.ID))
        //            .Select(jp => jp);
        //        if (result.Count() > 0)
        //        {
        //            foreach (var item in result)
        //            {
        //                UsersForJobPositionInDocumentModel jpuItem = new UsersForJobPositionInDocumentModel();
        //                jpuItem.JobPosition = Mapper.Map<JobPositionModel>(item);
        //                foreach (var dbUser in item.JobPositionUsers.Select(u => u.User))
        //                {
        //                    UserReadDocument user = new UserReadDocument();
        //                    user.User = Mapper.Map<UserModel>(dbUser);
        //                    user.DateRead = dbUser.ReadConfirmations.Where(rc => rc.DocumentID == documentId).Select(rc => rc.ReadDate).FirstOrDefault();
        //                    //foreach (var function1 in 
        //                    //    dbUser.ReadConfirmations.Where(rc => rc.DocumentID == documentId)
        //                    //    .Select(rc => rc)
        //                    //    .FirstOrDefault()
        //                    //    .ConfirmUserFunctions.Select(cuf => cuf))
        //                    //{
        //                    //    user.Functions += function1.JobPositionName + " ";
        //                    //}


        //                    var rc1 = dbUser.ReadConfirmations.Where(rc => rc.DocumentID == documentId)
        //                        .Select(rc => rc).FirstOrDefault();


        //                    if (rc1 != null)
        //                    {
        //                        foreach (var function1 in rc1.ConfirmUserFunctions.Select(cuf => cuf))
        //                        {
        //                            user.Functions += function1.JobPositionName + " ";
        //                        };
        //                    };


        //                    user.User.Password = "";
        //                    jpuItem.Users.Add(user);
        //                }
        //                jpu.Add(jpuItem);
        //            }
        //        }

        //        UsersForJobPositionInDocumentModel jpuItemManual = new UsersForJobPositionInDocumentModel();
        //        jpuItemManual.JobPosition = new JobPositionModel() {
        //            Name = "Ručně přidaní uživatelé",
        //            ID = 0
        //        };

        //        var usersDocument = db.Users.Where(u => usersID.Contains(u.ID)).Select(u => u);
        //        if (usersDocument.Count() > 0)
        //        {
        //            foreach (var userManual in usersDocument)
        //            {
        //                UserReadDocument userRead = new UserReadDocument();
        //                userRead.User = Mapper.Map<UserModel>(userManual);
        //                userRead.DateRead = userManual.ReadConfirmations
        //                    .Where(rc => rc.UserID == userManual.ID && rc.DocumentID == documentId)
        //                    .Select(rc => rc.ReadDate).FirstOrDefault();
        //                userRead.User.Password = "";
        //                jpuItemManual.Users.Add(userRead);
        //            }

        //            jpu.Add(jpuItemManual);
        //        }
        //    }

        //    return jpu;

        //}

        public void AddReadConfirms(int documentID, ICollection<UserJobModel> usersJob)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                foreach (var item in usersJob)
                {


                    ReadConfirmation newReadConfirm = new ReadConfirmation();

                    newReadConfirm.Created = DateTime.Now;
                    newReadConfirm.DocumentID = documentID;
                    newReadConfirm.JobPositionName = item.JobPositionName;
                    newReadConfirm.JobPositionID = item.JobPositionID;
                    newReadConfirm.UserID = item.UserID;

                    db.ReadConfirmations.Add(newReadConfirm);

                    db.SaveChanges();

                }

            }

        }

        public ReadConfirmCollectionModel GetReadConfirms(ReadConfirmFilter filter)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                var dbRC = db.ReadConfirmations.Where(rc => rc.DocumentID == filter.DocumentID).Select(rc => rc);
                ReadConfirmCollectionModel readConfirmCollection = new ReadConfirmCollectionModel();

                switch (filter.OrderBy)
                {
                    case "GetReadConfirms_NameA":
                        dbRC = dbRC.OrderBy(rc => rc.User.LastName);
                        break;
                    case "GetReadConfirms_NameD":
                        dbRC = dbRC.OrderByDescending(rc => rc.User.LastName);
                        break;
                    case "GetReadConfirms_JobPostionA":
                        dbRC = dbRC.OrderBy(rc => rc.JobPositionName);
                        break;
                    case "GetReadConfirms_JobPostionD":
                        dbRC = dbRC.OrderByDescending(rc => rc.JobPositionName);
                        break;
                    case "GetReadConfirms_CreatedA":
                        dbRC = dbRC.OrderBy(rc => EntityFunctions.TruncateTime(rc.Created));
                        break;
                    case "GetReadConfirms_CreatedD":
                        dbRC = dbRC.OrderByDescending(rc => EntityFunctions.TruncateTime(rc.Created));
                        break;
                    case "GetReadConfirms_ReadDateA":
                        dbRC = dbRC.OrderBy(rc => rc.ReadDate);
                        break;
                    case "GetReadConfirms_ReadDateD":
                        dbRC = dbRC.OrderByDescending(rc => rc.ReadDate);
                        break;
                    default:
                        // pro pripad ze nebude vyplneno order by
                        dbRC = dbRC;
                        break;

                }


                readConfirmCollection.Count = dbRC.Count();

                readConfirmCollection.ReadConfirms = Mapper.Map<ICollection<ReadConfirmModel>>(dbRC);

                //readConfirmCollection.ReadConfirms = Mapper.Map<ICollection<ReadConfirmModel>>(dbRC.Skip((filter.Page - 1) * filter.ItemsPerPage).Take(filter.ItemsPerPage));

                // LF 4.12.2017 
                readConfirmCollection.filter = filter;

                return readConfirmCollection;
            }
        }

        public ICollection<Item> GetUniqueAuthorsDocuments(DocumentFilter filter)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                // User1 Author
                //User Administrator
                var docColl = db.Documents
                .Where(rc => rc.StateID == filter.StateID && rc.Revision == "p" && rc.User.Active == true)
                .Select(d => d.AuthorID).Distinct();

                var userColl = db.Users.Where(u => docColl.Contains(u.ID))
                    .Select(u => new Item { ID = u.ID, Value = u.LastName + " " + u.FirstName })
                    .OrderBy(u => u.Value)
                    .ToList();

                int i = 1;
                foreach (var item in userColl)
                {
                    item.OrderBy = i;
                    i++;
                }

                return userColl;


            }
        }

        public ICollection<Item> GetUniqueAdministratorsDocuments(DocumentFilter filter)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                // User1 Author
                //User Administrator

                var docColl = db.Documents
                    .Where(rc => rc.StateID == filter.StateID && rc.Revision == "p" && rc.User.Active == true)
                    .Select(d => d.AdministratorID).Distinct();

                var userColl = db.Users.Where(u => docColl.Contains(u.ID))
                    .Select(u => new Item { ID = u.ID, Value = u.LastName + " " + u.FirstName })
                    .OrderBy(u => u.Value)
                    .ToList();

                int i = 1;
                foreach (var item in userColl)
                {
                    item.OrderBy = i;
                    i++;
                }

                return userColl;


            }
        }


        public ICollection<Item> GetUniqueReadConfirmsDocuments(DocumentFilter filter)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                // User1 Author
                //User Administrator

                var docColl = db.Documents
                    .Where(rc => rc.StateID == filter.StateID && rc.Revision == "p" && rc.User.Active == true)
                    .Select(d => d.ID);

                var readConfirmColl = db.ReadConfirmations.Where(rc => docColl.Contains(rc.DocumentID))
                    .Select(rc => rc.UserID).Distinct();

                var userColl = db.Users.Where(u => readConfirmColl.Contains(u.ID))
                    .Select(u => new Item { ID = u.ID, Value = u.LastName + " " + u.FirstName })
                    .OrderBy(u => u.Value)
                    .ToList();


                int i = 1;
                foreach (var item in userColl)
                {
                    item.OrderBy = i;
                    i++;
                }

                return userColl;


            }
        }

    }
    
}
