namespace FlightNode.Identity.Infrastructure.Migrations
{

    //internal sealed class Configuration : DbMigrationsConfiguration<Persistence.IdentityDbContext>
    //{
    //    public Configuration()
    //    {
    //        AutomaticMigrationsEnabled = false;
    //    }

    //    protected override void Seed(Persistence.IdentityDbContext context)
    //    {
    //        context.Roles.AddOrUpdate(r => r.Name,
    //            new Role { Name = "Administrator", Description = "Administrative user" },
    //            new Role { Name = "User", Description = "Regular user" }
    //        );

    //        var manager = new UserManager(new UserStore(context));

    //        var user = new User
    //        {
    //            UserName = "Admin",
    //            Email = "stephen@safnet.com"
    //        };

    //        manager.CreateAsync(user, "sys.Admin.9").Wait();
    //    }
    //}
}
