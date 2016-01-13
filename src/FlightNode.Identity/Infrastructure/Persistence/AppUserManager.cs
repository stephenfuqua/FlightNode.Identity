using FlightNode.Identity.Domain.Entities;
using FlightNode.Identity.Domain.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FlightNode.Identity.Infrastructure.Persistence
{

    public class AppUserManager : UserManager<User, int>, IUserPersistence
    {
        public AppUserManager(IUserStore<User, int> store)
        : base(store)
        {
        }
        

        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            var manager = new AppUserManager(new AppUserStore(context.Get<IdentityDbContext>()));

            // Configure validation logic for usernames 
            manager.UserValidator = new UserValidator<User, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords 
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Two-factor authenticadtion
            manager.RegisterTwoFactorProvider("EmailCode",
                new EmailTokenProvider<User, int>
                {
                    // TODO: subject should not be hard-coded to FlightNode. Need to be flexible
                    // enough to handle the project name.
                    Subject = "FlightNode Security Code",
                    BodyFormat = "Your security code is: {0}"
                });

            manager.EmailService = new EmailService();



            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<User, int>(
                        dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
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