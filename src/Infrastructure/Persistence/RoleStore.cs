using Microsoft.AspNet.Identity.EntityFramework;
using FlightNode.Identity.Domain.Entities;

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
