using DAL.Models;
using DAL.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace DAL.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetByIdAsync(string userId);

        Task<IdentityResult> UpdateAsync(ApplicationUser user);
    }
}