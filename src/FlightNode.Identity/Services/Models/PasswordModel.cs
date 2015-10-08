using System.ComponentModel.DataAnnotations;

namespace FlightNode.Identity.Services.Models
{
    /// <summary>
    /// Data transfer object for a user to change her own password.
    /// </summary>
    public class PasswordModel
    {
        /// <summary>
        /// The current password
        /// </summary>
        [DataType(DataType.Password)]
        [Required]
        public string CurrentPassword { get; set; }

        /// <summary>
        /// The desired new password
        /// </summary>
        [DataType(DataType.Password)]
        [Required]
        public string NewPassword { get; set; }
    }
}
