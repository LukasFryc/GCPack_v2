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
        public int JobPositionID { get; set; }
        public string Name { get; set; }
    }

}
