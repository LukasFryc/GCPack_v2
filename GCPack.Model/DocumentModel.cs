using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCPack.Model
{
    public enum DocumentAdminType
    {
        User,
        FromDocumentType
    }

    public class FileItem
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public string Size { get; set; }
    }

    public class DocumentModel
    {
        public DocumentModel()
        {
            FileItems = new HashSet<FileItem>();
        }

        public int ID { get; set; }
        public string Title { get; set; }
        public string Annotation { get; set; }
        public DocumentAdminType DocumentAdminType { get; set; }
        public int AdminID { get; set; }
        public bool CanEdit { get; set; }
        public bool CanRevision { get; set; }
        public ICollection<int> SelectedUsers { get; set; }
        public int DocumentTypeID { get; set; }
        public string DocumentNumber { get; set; }
        public string Revision { get; set; }
        public int StateID { get; set; }
        public int AdministratorID { get; set; }
        public Nullable<System.DateTime> EffeciencyDate { get; set; }
        public Nullable<System.DateTime> ReviewDate { get; set; }
        public Nullable<System.DateTime> NextReviewDate { get; set; }
        public ICollection<FileItem> FileItems { get; set; }
        public string DeleteFileItems { get; set; } // soubory ktere smazeme
    }








}
