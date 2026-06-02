using System.Security.Claims;

namespace BLL.Services.Abstractions
{
    public interface IRoleService
    {
        Task<List<Claim>> GetRoleClaimsAsync(string roleName);
    }
}
