using FlightNode.Identity.Domain.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using FlightNode.Identity.Services.Models;
using FlightNode.Common.Exceptions;

namespace FlightNode.Identity.Domain.Managers
{
    /// <summary>
    /// Domain logic for managing User Roles
    /// </summary>
    public class RoleDomainManager : IRoleManager
    {
        private readonly IRolePersistence _roleStore;

        /// <summary>
        /// Creates a new instance of <see cref="RoleDomainManager"/>.
        /// </summary>
        /// <param name="roleStore">An instance of <see cref="IRolePersistence"/>.</param>
        public RoleDomainManager(IRolePersistence roleStore)
        {
            if (roleStore == null)
            {
                throw new ArgumentNullException("roleStore");
            }

            _roleStore = roleStore;
        }

        /// <summary>
        /// Returns all Role records.
        /// </summary>
        /// <returns>IEnumerable of <see cref="RoleModel"/>.</returns>
        public IEnumerable<RoleModel> FindAll()
        {
            try
            {
                var records = _roleStore.Roles.ToList();

                return records.Select(x => new RoleModel
                {
                    Description = x.Description,
                    Id = x.Id,
                    Name = x.Name
                });

            }
            catch (Exception ex)
            {
                throw ServerException.HandleException<RoleDomainManager>(ex, "FindAll");
            }

        }
    }
}
