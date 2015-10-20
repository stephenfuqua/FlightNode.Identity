using System.ComponentModel.DataAnnotations;

namespace FlightNode.Identity.Services.Models
{
    /// <summary>
    /// Data-transfer object representing a system user
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// User ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// E-mail Address
        /// </summary>
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Primary Phone Number
        /// </summary>
        [Phone]
        public string PrimaryPhoneNumber { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Secondary Phone Number
        /// </summary>
        [Phone]
        public string SecondaryPhoneNumber { get; set; }

        /// <summary>
        /// Given ("first") Name
        /// </summary>
        [Required]
        [StringLength(50)]
        public string GivenName { get; set; }

        /// <summary>
        /// Family ("last") Name
        /// </summary>
        [Required]
        [StringLength(50)]
        public string FamilyName { get; set; }

        /// <summary>
        /// Password. Only used for initial creation. Ignored in PUT/update
        /// </summary>
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // TODO: will be used soon.

        //public IList<string> Roles { get; set; }

        //public IList<System.Security.Claims.ClaimsIdentity> Claims { get; set; }
    }
}
