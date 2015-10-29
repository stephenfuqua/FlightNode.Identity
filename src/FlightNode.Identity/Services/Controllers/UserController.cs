using FlightNode.Common.Exceptions;
using FlightNode.Identity.Domain.Interfaces;
using FlightNode.Identity.Domain.Logic;
using FlightNode.Identity.Services.Models;
using FligthNode.Common.Api.Controllers;
using Flurl;
using System;
using System.Security.Claims;
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

        // TODO: further limit who can take what actions. This class
        // should generally be used just for administration.


        /// <summary>
        /// Retrieves all active system users.
        /// </summary>
        /// <returns>Action result containing an array of users</returns>
        /// <example>
        /// GET: /api/v1/User
        /// </example>
        [Authorize]
        public IHttpActionResult Get()
        {
            try
            {
                var all = _manager.FindAll();
                return Ok(all);
            }
            catch (Exception ex)
            {
                return Handle(ex);
            }
        }

        /// <summary>
        /// Retrieves a single user by User Id value.
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>Action result containing the requested user, or status code 404 "not found".</returns>
        /// <example>
        /// GET: /api/v1/user/1
        /// </example>
        [Authorize]
        public IHttpActionResult Get(int id)
        {
            try
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
            }
            catch (UserException tue)
            {
                return Handle(tue);
            }
            catch (Exception ex)
            {
                return Handle(ex);
            }
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
        /// 
        //[Authorize(Roles = "Administrator")]   // the roles are not loading or something... because a user who actually has this role is being denied.
        [Authorize]
        public IHttpActionResult Post([FromBody]UserModel user)
        {
            //var identity = (User.Identity as ClaimsIdentity);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _manager.Create(user);
                var location = Request.RequestUri
                                      .ToString()
                                      .AppendPathSegment(result.UserId.ToString());

                return Created(location, result);
            }
            catch (UserException tue)
            {
                return Handle(tue);
            }
            catch (Exception ex)
            {
                return Handle(ex);
            }
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
        [Authorize]
        [Route("api/v1/user/changepassword/{id:int}")]
        public IHttpActionResult ChangePassword(int id, [FromBody]PasswordModel change)
        {
            // Todo ther's a lot of duplicated code here...
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _manager.ChangePassword(id, change);
            
                return NoContent();
            }
            catch (UserException tue)
            {
                return Handle(tue);
            }
            catch (Exception ex)
            {
                return Handle(ex);
            }

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
        [Authorize]
        public IHttpActionResult Put(int id, [FromBody]UserModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // For safety, override the message body's id with the input value
                user.UserId = id;
                _manager.Update(user);

                return NoContent();
            }
            catch (UserException tue)
            {
                return Handle(tue);
            }
            catch (Exception ex)
            {
                return Handle(ex);
            }
        }


        /// <summary>
        /// Soft-deletes (deactivates) a user from the system.
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>Action result with status code 204 "No content".</returns>
        /// <example>
        /// DELETE: api/v1/User/1
        /// </example>
        [Authorize]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                _manager.Deactivate(id);

                return NoContent();
            }
            catch (UserException tue)
            {
                return Handle(tue);
            }
            catch (Exception ex)
            {
                return Handle(ex);
            }
        }
    }
}
