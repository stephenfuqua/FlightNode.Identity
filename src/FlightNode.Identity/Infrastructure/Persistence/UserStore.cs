using FlightNode.Identity.Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

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
