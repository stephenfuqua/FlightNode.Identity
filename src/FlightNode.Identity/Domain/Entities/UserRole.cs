using FlightNode.Common.BaseClasses;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightNode.Identity.Domain.Entities
{
    public class UserRole : IdentityUserRole<int>, IEntity
    {
        [NotMapped]
        public int Id { get { return base.RoleId; } set { base.RoleId = value; } }
    }
}
