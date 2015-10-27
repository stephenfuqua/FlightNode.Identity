
using FlightNode.Identity.Domain.Entities;
using System.Linq;

namespace FlightNode.Identity.Domain.Interfaces
{
    public interface IRolePersistence
    {
        IQueryable<Role> Roles { get; }
    }
}
