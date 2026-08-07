using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data.Configurations
{
    public class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(u => u.RoleId);

            builder.Property(r=>r.RoleName).IsRequired().HasMaxLength(150);


            #region relations
            builder.HasMany(u=>u.userInRoles).WithOne(u=> u.Role).HasForeignKey(u=>u.RoleId);
            #endregion
        }
    }
}
