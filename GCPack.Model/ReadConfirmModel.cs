using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCPack.Model
{
    public class ReadConfirmModel
    {

        public int DocumentID { get; set; }

        public int UserID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int JobPositionID { get; set; }

        public string JobPositionName { get; set; }

        public DateTime? ReadDate { get; set; }
        public DateTime Created { get; set; }

    }
}
