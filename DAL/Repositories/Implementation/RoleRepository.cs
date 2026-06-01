using DAL.Context;
using DAL.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DAL.Repositories.Implementations
{
    public class RoleRepository : IRoleRepository
    {
        private readonly EmsContext _context;

        public RoleRepository(EmsContext context)
        {
            _context = context;
        }

        public async Task<List<Claim>> GetRoleClaimsAsync(string roleName)
        {
            var roleId = await _context.Roles
                .Where(r => r.Name == roleName)
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(roleId))
                return new List<Claim>();

            return await _context.RoleClaims
                .Where(rc => rc.RoleId == roleId)
                .Select(rc => new Claim(rc.ClaimType!, rc.ClaimValue!))
                .ToListAsync();
        }

        public async Task<string?> GetRoleIdByNameAsync(string roleName)
        {
            return await _context.Roles
                .Where(r => r.Name == roleName)
                .Select(r => r.Id)
                .FirstOrDefaultAsync();
        }
    }
}