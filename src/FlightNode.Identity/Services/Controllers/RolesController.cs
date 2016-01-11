using FlightNode.Identity.Domain.Interfaces;
using FlightNode.Identity.Services.Models;
using FligthNode.Common.Api.Controllers;
using System;
using System.Linq;
using System.Web.Http;

namespace FlightNode.Identity.Services.Controllers
{
    /// <summary>
    /// API Controller for User Roles
    /// </summary>
    public class RolesController : LoggingController
    {
        private readonly IRoleManager _roleManager;

        /// <summary>
        /// Creates a new instance of <see cref="RolesController"/>.
        /// </summary>
        /// <param name="roleManager">Instance of <see cref="IRoleManager"/></param>
        public RolesController(IRoleManager roleManager)
        {
            if (roleManager == null)
            {
                throw new ArgumentNullException("roleManager");
            }

            _roleManager = roleManager;
        }

        /// <summary>
        /// Retrieves all roles
        /// </summary>
        /// <returns>Action Result with a collection of <see cref="RoleModel"/></returns>
        [Authorize(Roles = "Administrator, Coordinator")]
        public IHttpActionResult Get()
        {
            return WrapWithTryCatch(() =>
            {
                var list = _roleManager.FindAll().ToList();
                if (list.Any())
                {
                    return Ok(list);
                }

                return NotFound();
            });
        }
    }
}
