using DAL.Context;
using DAL.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implementations
{
    public class RoleClaimRepository : IRoleClaimRepository
    {
        private readonly EmsContext _context;

        public RoleClaimRepository(EmsContext context)
        {
            _context = context;
        }

        public async Task<List<IdentityRoleClaim<string>>> GetAllRoleClaimsAsync()
        {
            return await _context.RoleClaims.ToListAsync();
        }
    }
}
