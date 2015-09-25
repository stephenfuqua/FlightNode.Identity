using FlightNode.Identity.Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace FlightNode.Identity.Infrastructure.Persistence
{
    public class IdentityDbContext : IdentityDbContext<User, Role, int, UserLogin, UserRole, UserClaim>
    {
        // TODO: extract an interface

        public IdentityDbContext()
            :base(Properties.Settings.Default.IdentityConnectionString)
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<UserRole>().ToTable("UserRoles");
            modelBuilder.Entity<UserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<UserClaim>().ToTable("UserClaims");
        }



        public static IdentityDbContext Create()
        {
            return new IdentityDbContext();
        }
    }
}
