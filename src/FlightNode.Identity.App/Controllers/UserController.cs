using FlightNode.Identity.Domain.Logic;

namespace FlightNode.Identity.App.Controllers
{
    public class ddUserController : FligthNode.Identity.Services.Controllers.UserController
    {
        public ddUserController(IUserLogic manager) : base(manager)
        {
        }
    }
}