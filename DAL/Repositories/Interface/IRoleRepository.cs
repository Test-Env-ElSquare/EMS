using System.Security.Claims;

namespace DAL.Repositories.Interface
{
    public interface IRoleRepository
    {
        Task<List<Claim>> GetRoleClaimsAsync(string roleName);

        Task<string?> GetRoleIdByNameAsync(string roleName);
    }
}