using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class UserInRole
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }


        #region Relations
        public Users User { get; set; }
        public Role Role { get; set; }
        #endregion
    }
}
