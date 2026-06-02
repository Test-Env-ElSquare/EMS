using Microsoft.AspNetCore.Identity;

namespace DAL.Repositories.Interface
{
    public interface IRoleClaimRepository
    {
        Task<List<IdentityRoleClaim<string>>> GetAllRoleClaimsAsync();
    }
}