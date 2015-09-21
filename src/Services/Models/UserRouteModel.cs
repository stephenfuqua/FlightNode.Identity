using System.ComponentModel.DataAnnotations;

namespace FlightNode.Identity.Services.Models
{
    public class UserModel
    {
        public int UserId { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string UserName { get; set; }

        [Phone]
        public string MobilePhoneNumber { get; set; }

        // Only used for initial password. Maybe sub-class UserDto
        // in order to add a password, just when creating / updating.
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //public IList<string> Roles { get; set; }

        //public IList<System.Security.Claims.ClaimsIdentity> Claims { get; set; }
    }
}
