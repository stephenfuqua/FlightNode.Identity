using FlightNode.Identity.Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FlightNode.Identity.Infrastructure.Persistence
{
    public class AppUserStore : UserStore<User, Role, int, UserLogin, UserRole, UserClaim>
    {
        public AppUserStore(IdentityDbContext context)
            : base(context)
        {
        }
    }
}
