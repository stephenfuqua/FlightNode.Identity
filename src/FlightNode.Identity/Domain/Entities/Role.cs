using FlightNode.Common.BaseClasses;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace FlightNode.Identity.Domain.Entities
{
    public class Role : IdentityRole<int, UserRole>, IEntity
    {
        [Required]
        [MaxLength(256)]
        public string Description { get; set; }
    }
}
