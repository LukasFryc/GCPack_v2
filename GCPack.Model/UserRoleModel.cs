using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCPack.Model
{
    public class UserRoleModel
    {
            public int UserRoleId { get; set; }
            public int UserID { get; set; }
            public short RoleId { get; set; }

            public virtual RoleModel Role { get; set; }
            public virtual UserModel User { get; set; }
        
    }
}
