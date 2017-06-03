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

    public class DocumentModel
    {
        public int DocumentID { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DocumentAdminType DocumentAdminType { get; set; }
        public int AdminID { get; set; }
        public bool CanEdit { get; set; }
        public bool CanRevision { get; set; }
        public string Revision { get; set; }
        public ICollection<int> SelectedUsers { get; set; }
    }
}
