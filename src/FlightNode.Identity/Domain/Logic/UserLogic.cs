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
        UserModel Create(UserModel input);
        void Update(UserModel input);
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
                throw new ArgumentNullException("manager");
            }

            _userManager = manager;
        }


        public void Deactivate(int id)
        {
            _userManager.SoftDelete(id);
        }

        public IEnumerable<UserModel> FindAll()
        {
            var records = _userManager.Users.Where(x => x.Active);

            var dtos = Map(records);

            return dtos;                
        }

        public UserModel FindById(int id)
        {
            var record = _userManager.FindByIdAsync(id).Result;

            // TODO: Is this fully hydrated? That is, does FindByIdAsync also populate 
            // roles & claims? If so, need to map those as well.

            var dto = Map(record);

            return dto;
        }

        private UserModel Map(User input)
        {
            return new UserModel
            {
                Email = input.Email,
                MobilePhoneNumber = input.MobilePhoneNumber,
                Password = string.Empty,
                PhoneNumber = input.PhoneNumber,
                UserId = input.Id,
                UserName = input.UserName,
                GivenName = input.GivenName,
                FamilyName = input.FamilyName
            };
        }

        private IEnumerable<UserModel> Map(IEnumerable<User> input)
        {
            foreach(var i in input)
            {
                yield return Map(i);
            }
        }
        
        public void Update(UserModel input)
        {
            var record = _userManager.FindByIdAsync(input.UserId).Result;

            record.UserName = input.Email;
            record.MobilePhoneNumber = input.MobilePhoneNumber;
            record.PhoneNumber = input.PhoneNumber;
            record.UserName = input.UserName;
            record.GivenName = input.GivenName;
            record.FamilyName = input.FamilyName;

            var result = _userManager.UpdateAsync(record).Result;
            if (!result.Succeeded)
            {
                throw UserException.FromMultipleMessages(result.Errors);
            }
        }

        public UserModel Create(UserModel input)
        {
            var record = Map<UserModel, User>(input);

            var result = _userManager.CreateAsync(record, input.Password).Result;
            if (result.Succeeded)
            {
                input.UserId = record.Id;
                return input;
            }
            else
            {
                throw UserException.FromMultipleMessages(result.Errors);
            }

        }

        public void ChangePassword(int id, PasswordModel change)
        {
            var result = _userManager.ChangePasswordAsync(id, change.CurrentPassword, change.NewPassword).Result;
            if (!result.Succeeded)
            {
                throw UserException.FromMultipleMessages(result.Errors);
            }
        }
    }
}
