using FlightNode.Identity.Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FlightNode.Identity.Infrastructure.Persistence
{

    public class RoleStore : RoleStore<Role, int, UserRole>
    {
        public RoleStore(IdentityDbContext context)
            : base(context)
        {
        }
    }
}
