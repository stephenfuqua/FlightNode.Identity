using FlightNode.Identity.Domain.Entities;
using FlightNode.Identity.Domain.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FlightNode.Identity.Infrastructure.Persistence
{

    public class AppRoleStore : RoleStore<Role, int, UserRole>, IRolePersistence
    {
        public AppRoleStore(IdentityDbContext context)
            : base(context)
        {
        }
    }
}
