using FlightNode.Common.BaseClasses;
using FlightNode.Common.Exceptions;
using FlightNode.Identity.Domain.Entities;
using FlightNode.Identity.Domain.Interfaces;
using FlightNode.Identity.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlightNode.Identity.Domain.Logic
{
    public interface IUserLogic
    {
        IEnumerable<UserModel> FindAll();
        UserModel FindById(int id);
        UserModel Save(UserModel input);
        void Deactivate(int id);
        void ChangePassword(int id, PasswordModel change);
    }

    public class UserLogic : DomainLogic, IUserLogic
    {
        private IUserManager _userManager;

        public UserLogic(IUserManager manager)
        {
            if (manager == null)
            {
                throw new ArgumentException("manager");
            }

            _userManager = manager;

            //_userManager = new UserManager(new UserStore(new IdentityDbContext()));
        }


        public void Deactivate(int id)
        {
            _userManager.SoftDelete(id);
        }

        public IEnumerable<UserModel> FindAll()
        {
            var records = _userManager.Users.Where(x => x.Active);

            var dtos = Map<User, UserModel>(records);

            return dtos;                
        }

        public UserModel FindById(int id)
        {
            var record = _userManager.FindByIdAsync(id).Result;
            
            // Is this fully hydrated?

            var dto = Map<User, UserModel>(record);

            return dto;
        }

        public UserModel Save(UserModel input)
        {
            // Need to re-assign roles / claims ??

            var record = Map<UserModel, User>(input);

            // Do I need to load the original into EF first?
            //var original = _ternRepository.FindByIdAsync(input.UserId).Result;

            
            if (input.UserId < 1)
            {
                input.UserId = SaveNew(record, input.Password);
                return input;
            }
            else
            {
                UpdateExisting(record);
                return input;
            }
        }

        private void UpdateExisting(User input)
        {
            var result = _userManager.UpdateAsync(input).Result;
            if (!result.Succeeded)
            {
                throw UserException.FromMultipleMessages(result.Errors);
            }
        }

        private int SaveNew(User record, string password)
        {
            var result = _userManager.CreateAsync(record, password).Result;
            if (result.Succeeded)
            {
                // Does this record now have the UserId in it?
                return record.Id;
            }
            else
            {
                throw UserException.FromMultipleMessages(result.Errors);
            }

        }

        public void ChangePassword(int id, PasswordModel change)
        {
            var result = _userManager.ChangePasswordAsync(id, change.OldPassword, change.NewPassword).Result;
            if (!result.Succeeded)
            {
                throw UserException.FromMultipleMessages(result.Errors);
            }
        }
    }
}
