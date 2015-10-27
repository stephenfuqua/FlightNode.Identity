using FlightNode.Identity.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightNode.Identity.Domain.Interfaces
{
    public interface IRoleManager
    {
        IEnumerable<RoleModel> FindAll();
    }
}
