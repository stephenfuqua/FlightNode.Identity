using FlightNode.Common.Exceptions;
using FlightNode.Identity.Domain.Logic;
using FlightNode.Identity.Services.Models;
using FligthNode.Common.Api.Controllers;
using Flurl;
using System;
using System.Web.Http;

namespace FligthNode.Identity.Services.Controllers
{
    public class UserController : LoggingController
    {
        private readonly IUserLogic _userLogic;

        public UserController(IUserLogic manager)
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }

            _userLogic = manager;
        }


        // TODO: add in bearer token evaluation to ensure only valid users are accessing the API

        // GET: api/User
        [Authorize]
        public IHttpActionResult Get()
        {
            try
            {
                var all = _userLogic.FindAll();
                return Ok(all);
            }
            catch (Exception ex)
            {
                return Handle(ex);
            }
        }

        // GET: api/User/5
        [Authorize]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var result = _userLogic.FindById(id);
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

        // POST: api/User
        [Authorize]
        public IHttpActionResult Post([FromBody]UserModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _userLogic.Create(user);
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

        [HttpPost]
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
                _userLogic.ChangePassword(id, change);

                return Ok();
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

        // PUT: api/User/5
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
                _userLogic.Update(user);

                return Ok();
            }
            catch(UserException tue)
            {
                return Handle(tue);
            }
            catch (Exception ex)
            {
                return Handle(ex);
            }
        }

        // DELETE: api/User/5
        [Authorize]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                _userLogic.Deactivate(id);
                return Ok();
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
