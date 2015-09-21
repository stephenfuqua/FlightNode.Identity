using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using FlightNode.Common.BaseClasses;

namespace FlightNode.Identity.Domain.Entities
{
    public class User : IdentityUser<int, UserLogin, UserRole, UserClaim>, IEntity
    {
        public bool Active { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string MobilePhoneNumber { get; set; }

        public User()
        {
            Active = true;
        }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, int> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in
            // CookieAuthenticationOptions.AuthenticationType 

            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            
            // Add custom user claims here 

            return userIdentity;
        }
    }


}
