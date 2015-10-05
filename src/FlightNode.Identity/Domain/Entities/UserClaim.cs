using FlightNode.Common.BaseClasses;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FlightNode.Identity.Domain.Entities
{
    public class UserClaim : IdentityUserClaim<int>, IEntity
    {
    }
}
