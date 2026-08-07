using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Role: BaseEntity
    {
        public int RoleId { get; set; }
        public string  RoleName { get; set; }


        #region relations
        public ICollection<UserInRole> userInRoles { get; set; }
        #endregion
    }
}
