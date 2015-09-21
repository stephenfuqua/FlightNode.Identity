using FlightNode.Identity.Domain.Entities;
using FlightNode.Identity.Domain.Interfaces;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace FlightNode.Identity.Infrastructure.Persistence
{

    public class UserManager : UserManager<User, int>, IUserManager
    {
        public UserManager(IUserStore<User, int> store)
        : base(store)
        {
        }
        

        // This is not functionally useful yet. Need to get away from IOwinContext.
        //public static UserManager Create(
        //    IdentityFactoryOptions<UserManager> options, IOwinContext context)
        //{
        //    var manager = new UserManager(new UserStore(context.Get<IdentityDbContext>()));

        //    // Configure validation logic for usernames 
        //    manager.UserValidator = new UserValidator<User, int>(manager)
        //    {
        //        AllowOnlyAlphanumericUserNames = false,
        //        RequireUniqueEmail = true
        //    };

        //    // Configure validation logic for passwords 
        //    manager.PasswordValidator = new PasswordValidator
        //    {
        //        RequiredLength = 6,
        //        RequireNonLetterOrDigit = true,
        //        RequireDigit = true,
        //        RequireLowercase = true,
        //        RequireUppercase = true,
        //    };

        //    // Configure user lockout defaults
        //    manager.UserLockoutEnabledByDefault = true;
        //    manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
        //    manager.MaxFailedAccessAttemptsBeforeLockout = 5;

        //    // Two-factor authenticadtion
        //    manager.RegisterTwoFactorProvider("EmailCode",
        //        new EmailTokenProvider<User, int>
        //        {
        //            Subject = "TERN Security Code",
        //            BodyFormat = "Your security code is: {0}"
        //        });

        //    manager.EmailService = new EmailService();



        //    var dataProtectionProvider = options.DataProtectionProvider;
        //    if (dataProtectionProvider != null)
        //    {
        //        manager.UserTokenProvider =
        //            new DataProtectorTokenProvider<User, int>(
        //                dataProtectionProvider.Create("ASP.NET Identity"));
        //    }
        //    return manager;
        //}

        public void SoftDelete(int id)
        {
            //var input = new User
            //{
            //    Id = id,
            //    Active = false
            //};

            //var expressions = new List<Expression<Func<User, object>>>
            //     {
            //         x => x.Active
            //     };

            //try
            //{
            //    DbContext.Entry(input).SetModified(expressions);

            //    var rowcount = DbContext.SaveChanges();
            //    if (rowcount != 1)
            //    {
            //        // TODO: this should really be in a transaction 
            //        throw ServerException.UpdateFailed<User>("SoftDelete", id);
            //    }
            //}
            //catch (ServerException)
            //{
            //    throw;
            //}
            //catch (Exception ex)
            //{
            //    throw ServerException.HandleException<User>(ex, "SoftDelete", id);
            //}
        }
        
    }


    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // TODO: Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }
}