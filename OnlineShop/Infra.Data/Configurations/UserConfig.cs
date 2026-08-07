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
    public class UserConfig : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.HasKey(u=>u.UserId);

            builder.Property(u=>u.Username).IsRequired().HasMaxLength(150);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(200);
            builder.Property(u => u.Mobile).HasMaxLength(50);
            builder.Property(u => u.Password).IsRequired().HasMaxLength(400);

            builder.HasIndex(p=>p.Email).IsUnique();
            builder.HasIndex(p => p.Mobile).IsUnique();
            builder.HasIndex(p => p.Username).IsUnique();



            #region relations
            builder.HasMany(u=>u.userInRoles).WithOne(r => r.User).HasForeignKey(u=>u.UserId);
            #endregion
        }
    }
}
