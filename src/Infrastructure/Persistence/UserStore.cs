using Microsoft.AspNet.Identity.EntityFramework;
using FlightNode.Identity.Domain.Entities;

namespace FlightNode.Identity.Infrastructure.Persistence
{
    public class UserStore : UserStore<User, Role, int, UserLogin, UserRole, UserClaim>
    {
        public UserStore(IdentityDbContext context)
            : base(context)
        {
        }
    }
}
