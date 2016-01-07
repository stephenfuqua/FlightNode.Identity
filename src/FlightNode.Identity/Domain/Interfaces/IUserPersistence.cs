using FlightNode.Identity.Domain.Entities;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FlightNode.Identity.Domain.Interfaces
{
    public interface IUserPersistence
    {
        Task<User> FindByIdAsync(int id);
        Task<IdentityResult> UpdateAsync(User user);
        Task<IdentityResult> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<IList<Claim>> GetClaimsAsync(int userId);
        Task<IList<string>> GetRolesAsync(int userId);
        Task<IdentityResult> CreateAsync(User user, string password);
        Task<IdentityResult> AddClaimAsync(int userId, Claim claim);
        Task<IdentityResult> AddToRolesAsync(int userId, params string[] roles);
        IQueryable<User> Users { get; }
        Task<string> GeneratePasswordResetTokenAsync(int userId);
        Task<IdentityResult> ResetPasswordAsync(int userId, string token, string newPassword);
        Task<IdentityResult> RemoveFromRolesAsync(int userId, params string[] roles);
    }

}
