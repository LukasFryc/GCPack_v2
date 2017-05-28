using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCPack.Model
{
    public class RoleModel
    {
        public RoleModel()
        {
            this.UserRoles = new HashSet<UserRoleModel>();
        }

        public short RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleCode { get; set; }
        public string RoleDescription { get; set; }
        public virtual ICollection<UserRoleModel> UserRoles { get; set; }
    }
}
