using BLL.Services.Abstractions;
using DAL.Models.Identity;
using DAL.Repositories.Implementation;
using DAL.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


namespace BLL.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleService(IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }

        public async Task<List<Claim>> GetRoleClaimsAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                throw new KeyNotFoundException($"Role '{roleName}' not found");

            var claims = await _roleManager.GetClaimsAsync(role);

            return claims.ToList();
        }
    }
}
