namespace BLL.Services.Abstractions
{
    public interface IAuthService
    {
        Task<AuthServiceResult> LoginAsync(LoginModel model);
        Task<AuthServiceResult> RegisterAsync(RegisterFactoryModel model, CancellationToken ct);

        Task<AuthServiceResult> MyProfileAsync(ClaimsPrincipal user);
        Task<AuthServiceResult> UpdateMyProfileAsync(ClaimsPrincipal user, UpdateUserProfileDto model);
        Task<AuthServiceResult> ChangePasswordAsync(ClaimsPrincipal user, ChangePwDto changePwDto);

        Task<AuthServiceResult> AddRoleAsync(AddRoleDTO model);
        Task<AuthServiceResult> GetRoleDetailsAsync(RoleFilterRequest? filter);
        Task<AuthServiceResult> GetAllRolesAsync();
        Task<AuthServiceResult> UpdateRoleAsync(UpdateRoleDto model);
        Task<AuthServiceResult> DeleteRoleAsync(string roleName);

        Task<AuthServiceResult> GetAllUsersWithRolesAsync(string? role, string? email);
        Task<AuthServiceResult> UpdateUserByAdminAsync(UpdateUserDto model);
        Task<AuthServiceResult> DeleteUserAsync(string userId);

        Task<AuthServiceResult> ForgotPasswordAsync(ForgotPasswordDto model);
        Task<AuthServiceResult> ValidateOtpAsync(VerifyOtpDto model);
        Task<AuthServiceResult> ResetPasswordAsync(ResetPasswordDto model);

        Task<AuthServiceResult> AddClaimAsync(List<AddClaimDto> model);
        //Task<AuthServiceResult> GetAllSystemClaimsAsync(ClaimsPrincipal user);
    }
}
