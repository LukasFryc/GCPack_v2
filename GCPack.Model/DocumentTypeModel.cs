using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCPack.Model
{
    public class DocumentTypeModel
    {
        public DocumentTypeModel()
        {
            this.Documents = new HashSet<DocumentModel>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> AdministratorID { get; set; }
        public Nullable<int> AuthorizinOfficerID { get; set; }
        public virtual ICollection<DocumentModel> Documents { get; set; }
        public string AdministratorName { get; set; }
        public int? OrderBy { get; set; }
        public int ValidityInYears { get; set; }
        public bool IsRequiredFillListOfPersons { get; set; }
        public bool AutomaticNumberingOfDocuments { get; set; }
        public string NumberingOfDocumentPrefix { get; set; }
        public string NumberingOfDocumentSeparator { get; set; }
        public int NumberingOfDocumentLength { get; set; }
        public int LastNumberOfDocument { get; set; }
    }
}
