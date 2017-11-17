using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCPack.Model
{

    public class UserReadDocument
    {
        public DateTime? DateRead { get; set; }
        public UserModel User { get; set; }
        public string Functions { get; set; }
    }

    public class UsersInDocument
    {
        public UsersInDocument()
        {
            this.AllUsers = new HashSet<UserModel>();
            this.UsersRead = new HashSet<UserReadDocument>();
        }
        public int DocumentID { get; set; }
        public ICollection<UserModel> AllUsers { get; set; }
        public ICollection<UserReadDocument> UsersRead { get; set; }
    }
}
