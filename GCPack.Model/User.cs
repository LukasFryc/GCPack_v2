using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCPack.Model
{
    public class UserModel
    {
        public UserModel()
        {
            /*
            this.DocumentTypes = new HashSet<DocumentTypeModel>();
            this.LogEvents = new HashSet<LogEventModel>();
            this.Signatures = new HashSet<SignatureModel>();
            this.UserRoles = new HashSet<UserRoleModel>();
            */
        }

        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public bool Active { get; set; }
        public Nullable<int> ManagerID { get; set; }
        public Nullable<int> AdministratorID { get; set; }
        public Nullable<int> JobPositionID { get; set; }
        /*
        public virtual ICollection<DocumentTypeModel> DocumentTypes { get; set; }
        public virtual JobPositionModel JobPosition { get; set; }
        public virtual ICollection<LogEventModel> LogEvents { get; set; }
        public virtual ICollection<SignatureModel> Signatures { get; set; }
        */
        public string Roles { get; set; }
        
    }
}
