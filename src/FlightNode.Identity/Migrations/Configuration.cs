using FlightNode.Identity.Domain.Entities;
using FlightNode.Identity.Infrastructure.Persistence;
using System.Data.Entity.Migrations;

namespace FlightNode.Identity.Migrations
{

    internal sealed class Configuration : DbMigrationsConfiguration<FlightNode.Identity.Infrastructure.Persistence.IdentityDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FlightNode.Identity.Infrastructure.Persistence.IdentityDbContext context)
        {
            context.Roles.AddOrUpdate(r => r.Name,
            new Role { Name = "Administrator", Description = "Administrative user" },
            new Role { Name = "User", Description = "Regular user" }
            );

            var manager = new UserManager(new UserStore(context));

            var user = new User
            {
                UserName = "asdf",
                Email = "ab@asfddsdfs.com",
                GivenName = "Juana",
                FamilyName = "Coneja"
            };

            manager.CreateAsync(user, "dirigible1").Wait();
        }
    }
}
