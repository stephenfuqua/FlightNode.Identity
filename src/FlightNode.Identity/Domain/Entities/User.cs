using FlightNode.Common.BaseClasses;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FlightNode.Identity.Domain.Entities
{
    public class User : IdentityUser<int, UserLogin, UserRole, UserClaim>, IEntity
    {
        public bool Active { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string MobilePhoneNumber { get; set; }

        [Required]
        [StringLength(50)]  
        public string GivenName { get; set; }

        [Required]
        [StringLength(50)]
        public string FamilyName { get; set; }

        public User()
        {
            Active = true;
        }

        // TODO: refactor this to use our custom UserManager, with separated interface. In fact, 
        // what is the value of having this here? Comes from sample code.
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
