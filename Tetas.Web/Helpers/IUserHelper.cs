namespace Tetas.Web.Helpers
{
    using Common.ViewModels;
    using Domain.Entities;
    using Microsoft.AspNetCore.Identity;
    using System.Threading.Tasks;

    public interface IUserHelper
    {
        Task<ApplicationUser> GetUserByEmailAsync(string email);
          
        Task<IdentityResult> AddUserAsync(ApplicationUser user, string password);

        Task<SignInResult> LoginAsync(LoginModel model);

        Task LogoutAsync();

        Task<IdentityResult> UpdateUserAsync(ApplicationUser user);

        Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string oldPassword, string newPassword);

        Task<SignInResult> ValidatePasswordAsync(ApplicationUser user, string password);

        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(ApplicationUser user, string roleName);

        Task<bool> IsUserInRoleAsync(ApplicationUser user, string roleName);

        Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);

        Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token);

        Task<ApplicationUser> GetUserByIdAsync(string userId);

        Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);

        Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string password);

    }
}
