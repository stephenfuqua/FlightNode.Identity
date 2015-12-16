
using FlightNode.Identity.Domain.Interfaces;
using FlightNode.Identity.Services.Models;
using FligthNode.Common.Api.Controllers;
using Flurl;
using System;
using System.Web.Http;

namespace FligthNode.Identity.Services.Controllers
{
    /// <summary>
    /// API Controller for User records
    /// </summary>
    public class UserController : LoggingController
    {
        private readonly IUserDomainManager _manager;

        /// <summary>
        /// Creates a new instance of <see cref="UserController"/>
        /// </summary>
        /// <param name="manager">Instance of <see cref="IUserDomainManager"/></param>
        public UserController(IUserDomainManager manager)
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }

            _manager = manager;
        }


        // TODO: ensure that bearer token expirations are being honored.

        
        /// <summary>
        /// Retrieves all active system users.
        /// </summary>
        /// <returns>Action result containing an array of users</returns>
        /// <example>
        /// GET: /api/v1/User
        /// </example>
        [Authorize(Roles = "Administrator, Coordinator")]
        public IHttpActionResult Get()
        {
            return WrapWithTryCatch(() =>
            {
                var all = _manager.FindAll();
                return Ok(all);
            });
        }

        /// <summary>
        /// Retrieves a single user by User Id value.
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>Action result containing the requested user, or status code 404 "not found".</returns>
        /// <example>
        /// GET: /api/v1/user/1
        /// </example>
        [Authorize(Roles = "Administrator, Coordinator")]
        public IHttpActionResult Get(int id)
        {
            return WrapWithTryCatch(() =>
            {
                var result = _manager.FindById(id);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound();
                }
            });
        }

        /// <summary>
        /// Adds a new user to the system.
        /// </summary>
        /// <param name="user"><see cref="UserModel"/></param>
        /// <returns>Action result containing the created user record (including the new ID), with status code 201 "created".</returns>
        /// <example>
        /// POST: /api/v1/user
        /// {
        ///   "userName": "dirigible@asfddfsdfs.com",
        ///   "givenName": "Juana",
        ///   "familyName": "Coneja",
        ///   "email": "dirigible@asfddfsdfs.com",
        ///   "primaryPhoneNumber": "555-555-5555",
        ///   "secondaryPhoneNumber": "(555) 555-5554",
        ///   "password": "deerEatRabbits?"
        /// }
        /// </example>
        [Authorize(Roles = "Administrator, Coordinator")]
        public IHttpActionResult Post([FromBody]UserModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return WrapWithTryCatch(() =>
            {

                var result = _manager.Create(user);

                // TODO: is not saving Roles

                var location = Request.RequestUri
                                      .ToString()
                                      .AppendPathSegment(result.UserId.ToString());

                return Created(location, result);
            });
        }



        // TODO: create an administrative way to change password, which would
        // not require knowing the old password, but would require being in the 
        // proper administrative role.

        /// <summary>
        /// Changes a user's password
        /// </summary>
        /// <param name="id">User Id</param>
        /// <param name="change"><see cref="PasswordModel"/></param>
        /// <returns>Action result with status code 204 "No content".</returns>
        /// <example>
        /// PUT: api/v1/user/changepassword/1
        /// {
        ///   "oldPassword": "deerEatRabbits?",
        ///   "newPassword": "notUsually."
        /// }
        /// </example>
        [HttpPut]
        [Authorize(Roles = "Administrator, Coordinator")]
        [Route("api/v1/user/changepassword/{id:int}")]
        public IHttpActionResult ChangePassword(int id, [FromBody]PasswordModel change)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return WrapWithTryCatch(() =>
            {
                _manager.ChangePassword(id, change);

                return NoContent();
            });

        }

        /// <summary>
        /// Update an existing system user.
        /// </summary>
        /// <param name="id">User Id</param>
        /// <param name="user"><see cref="UserModel"/></param>
        /// <returns>Action result with status code 204 "No content".</returns>
        /// <remarks>
        /// Does not change the password.
        /// </remarks>
        /// <example>
        /// PUT: /api/v1/user/1
        /// {
        ///   "userId": 1,
        ///   "userName": "dirigible@asfddfsdfs.com",
        ///   "givenName": "Juana",
        ///   "familyName": "Coneja",
        ///   "email": "dirigible@asfddfsdfs.com",
        ///   "phoneNumber": "555-555-5555",
        ///   "mobilePhoneNumber": "(555) 555-5554",
        ///   "password": "will be ignored"
        /// }
        /// </example>
        [Authorize(Roles = "Administrator, Coordinator")]
        public IHttpActionResult Put(int id, [FromBody]UserModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return WrapWithTryCatch(() =>
            {
                // For safety, override the message body's id with the input value
                user.UserId = id;
                _manager.Update(user);

                return NoContent();
            });
        }


        /// <summary>
        /// Soft-deletes (deactivates) a user from the system.
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>Action result with status code 204 "No content".</returns>
        /// <example>
        /// DELETE: api/v1/User/1
        /// </example>
        [Authorize(Roles = "Administrator, Coordinator")]
        public IHttpActionResult Delete(int id)
        {
            return WrapWithTryCatch(() =>
            {
                _manager.Deactivate(id);

                return NoContent();
            });
        }
    }
}
