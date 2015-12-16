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

        public string DisplayName {  get { return GivenName + " " + FamilyName; } }

        public User()
        {
            Active = true;
        }

        /// <summary>
        /// Creates the identity object used for transmitting a token to a client.
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="authenticationType"></param>
        /// <returns>User's claims</returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, int> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in
            // CookieAuthenticationOptions.AuthenticationType 

            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            
            // Custom claims
            userIdentity.AddClaim(new Claim("displayName", this.DisplayName));

            return userIdentity;
        }
    }


}
