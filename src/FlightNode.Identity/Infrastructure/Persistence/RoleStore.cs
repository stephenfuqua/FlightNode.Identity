using FlightNode.Identity.Domain.Entities;
using FlightNode.Identity.Domain.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FlightNode.Identity.Infrastructure.Persistence
{

    public class RoleStore : RoleStore<Role, int, UserRole>, IRolePersistence
    {
        public RoleStore(IdentityDbContext context)
            : base(context)
        {
        }
    }
}
