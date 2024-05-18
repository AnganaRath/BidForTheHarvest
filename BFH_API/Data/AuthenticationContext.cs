using HUSP_API.Models.Authentication;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HUSP_API.Data
{
    public class AuthenticationContext : DbContext
    {
        public AuthenticationContext(DbContextOptions<AuthenticationContext> dbContextOptions)
       : base(dbContextOptions)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    optionsBuilder.UseSqlServer(@"Server=IT-SW30;Database=SP_USER_AUTH;Trusted_Connection=True;User ID=sa; Password=123456; MultipleActiveResultSets=True;");
                
            //}
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<UserTask> UserTasks { get; set; }
        public DbSet<UserAction> UserActions { get; set; }

        public DbSet<RoleTaskAction> RoleTaskActions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

    }
}
