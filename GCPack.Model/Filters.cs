using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCPack.Model
{
    public class Filter
    {
        public int Page { get; set; }
        public int ItemPerPage { get; set; }
        public string OrderBy { get; set; }
    }

    public class UserFilter : Filter 
    {
        public UserFilter()
        {
            this.ExcludedUsersId = new int[] { };
            this.JobPositionIDs = new HashSet<int>();
        }
        public ICollection<int> JobPositionIDs { get; set; }
        public string Name { get; set; }
        public int[] ExcludedUsersId { get; set; }
    }

    public class DocumentFilter : Filter
    {
        public int ForUserID { get; set; }
        public int DocumentID { get; set; }
        public string Name { get; set; }
        public string AdministratorName { get; set; }
        public string Number { get; set; }
        public DateTime ToDateReview { get; set; } // napriklad filtrovani podle datumu revize
        public DateTime FromDateReview { get; set; }
        public int DocumentTypeID { get; set; }
    }

}
