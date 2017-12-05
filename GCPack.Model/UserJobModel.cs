using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCPack.Model
{
    public class UserJobCollectionModel
    {
        public UserJobCollectionModel()
        {
            UserJobs = new HashSet<UserJobModel>();
        }
        public ICollection<UserJobModel> UserJobs { get; set; }
        public int Count { get; set; }

        public UserFilter filter { get; set; }
    }

    public class UserJobModel
        {
            public int UserID { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public int JobPositionID { get; set; }

            public string JobPositionName { get; set; }
        
        }
}
