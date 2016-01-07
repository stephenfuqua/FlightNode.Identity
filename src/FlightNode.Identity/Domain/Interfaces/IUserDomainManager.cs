using FlightNode.Identity.Services.Models;
using System.Collections.Generic;

namespace FlightNode.Identity.Domain.Interfaces
{
    public interface IUserDomainManager
    {
        IEnumerable<UserModel> FindAll();
        UserModel FindById(int id);
        UserModel Create(UserModel input);
        void Update(UserModel input);
        void ChangePassword(int id, PasswordModel change);
        void AdministrativePasswordChange(int userId, string newPassword);
    }

}
