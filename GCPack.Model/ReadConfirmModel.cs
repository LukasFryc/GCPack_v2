using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCPack.Model
{
    public class ReadConfirmCollectionModel
    {
        public ReadConfirmCollectionModel()
        {
            ReadConfirms = new HashSet<ReadConfirmModel>();
        }
        public ICollection<ReadConfirmModel> ReadConfirms { get; set; }
        public int Count { get; set; }

        // LF 4.12.2017 
        // urceno k navratu OrderBy do view, nasatveno v repository z filtru pro hledani
        // ve view je ouzito pro urceni defaultniho trideni 
        public ReadConfirmFilter filter { get; set; }
    }

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
