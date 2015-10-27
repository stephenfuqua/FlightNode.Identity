
using System.ComponentModel.DataAnnotations;

namespace FlightNode.Identity.Services.Models
{
    /// <summary>
    /// Messaging model for user roles.
    /// </summary>
    public class RoleModel
    {
        /// <summary>
        /// Role Name.
        /// </summary>
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        /// <summary>
        /// Longer description of the role.
        /// </summary>
        [Required]
        [MaxLength(256)]
        public string Description { get; set; }

        /// <summary>
        /// Auto-increment identifier.
        /// </summary>
        public int Id { get; set; }
    }
}
