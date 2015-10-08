using FlightNode.Common.BaseClasses;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FlightNode.Identity.Domain.Entities
{
    public class Role : IdentityRole<int, UserRole>, IEntity
    {
        public string Description { get; set; }
    }
}
