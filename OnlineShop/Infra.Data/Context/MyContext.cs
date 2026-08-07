using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data.Context
{

        public class MyContext(DbContextOptions<MyContext> options) :
        DbContext(options)
        {
        public DbSet<Users> Users { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductGallery> ProductGallery { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserInRole> userInRoles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            base.OnModelCreating(modelBuilder);
        }

    }

}
