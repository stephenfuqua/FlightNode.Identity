using System.ComponentModel.DataAnnotations;

namespace FlightNode.Identity.Services.Models
{
    public class PasswordModel
    {
        [DataType(DataType.Password)]
        [Required]
        public string OldPassword { get; set; }


        [DataType(DataType.Password)]
        [Required]
        public string NewPassword { get; set; }
    }
}
