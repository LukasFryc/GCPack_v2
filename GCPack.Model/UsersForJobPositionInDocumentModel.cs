using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCPack.Model
{
    // seznam vsech pracovnich pozic a jejich uzivatelu s datumem seznameni se
    // v dokumentu
    public class UsersForJobPositionInDocumentModel
    {
        public UsersForJobPositionInDocumentModel()
        {
            Users = new HashSet<UserReadDocument>();
        }
        public JobPositionModel JobPosition { get; set; }
        public ICollection<UserReadDocument> Users {get;set;}
    }
}
