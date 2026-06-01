using BLL.DTOs.Auth;
using BLL.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMSProject.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return ToActionResult(await _authService.LoginAsync(model));
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> Register([FromBody] RegisterFactoryModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            return ToActionResult(await _authService.RegisterAsync(model, ct));
        }

        [HttpGet("MyProfile")]
        public async Task<IActionResult> MyProfile()
        {
            return ToActionResult(await _authService.MyProfileAsync(User));
        }

        [HttpPut("UpdateMyProfile")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateUserProfileDto model)
        {
            return ToActionResult(await _authService.UpdateMyProfileAsync(User, model));
        }

        [HttpPut("MyProfile/ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePwDto changePwDto)
        {
            return ToActionResult(await _authService.ChangePasswordAsync(User, changePwDto));
        }

        [HttpPost("AddRoles")]
        public async Task<IActionResult> AddRole([FromBody] AddRoleDTO model)
        {
            return ToActionResult(await _authService.AddRoleAsync(model));
        }

        [HttpPost("GetRoleDetails")]
        public async Task<IActionResult> GetRoleDetails([FromBody] RoleFilterRequest? filter)
        {
            return ToActionResult(await _authService.GetRoleDetailsAsync(filter));
        }

        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            return ToActionResult(await _authService.GetAllRolesAsync());
        }

        [HttpPut("UpdateRole")]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleDto model)
        {
            return ToActionResult(await _authService.UpdateRoleAsync(model));
        }

        [HttpDelete("DeleteRole/{roleName}")]
        public async Task<IActionResult> DeleteRole([FromRoute] string roleName)
        {
            return ToActionResult(await _authService.DeleteRoleAsync(roleName));
        }

        [HttpGet("GetAllUsersWithRoles")]
        public async Task<IActionResult> GetAllUsersWithRoles(string? role, string? email)
        {
            return ToActionResult(await _authService.GetAllUsersWithRolesAsync(role, email));
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUserByAdmin([FromBody] UpdateUserDto model)
        {
            return ToActionResult(await _authService.UpdateUserByAdminAsync(model));
        }

        [HttpDelete("DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string userId)
        {
            return ToActionResult(await _authService.DeleteUserAsync(userId));
        }

        [AllowAnonymous]
        [HttpPost("password/forget")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            return ToActionResult(await _authService.ForgotPasswordAsync(model));
        }

        [AllowAnonymous]
        [HttpPost("password/validate-otp")]
        public async Task<IActionResult> ValidateOtp([FromBody] VerifyOtpDto model)
        {
            return ToActionResult(await _authService.ValidateOtpAsync(model));
        }

        [AllowAnonymous]
        [HttpPost("password/reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            return ToActionResult(await _authService.ResetPasswordAsync(model));
        }

        [HttpPost("AddClaim")]
        public async Task<IActionResult> AddClaim([FromBody] List<AddClaimDto> model)
        {
            return ToActionResult(await _authService.AddClaimAsync(model));
        }

        //[HttpGet("GetAllClaims")]
        //public async Task<IActionResult> GetAllSystemClaims()
        //{
        //    return ToActionResult(await _authService.GetAllSystemClaimsAsync(User));
        //}

        private IActionResult ToActionResult(AuthServiceResult result)
        {
            if (result.StatusCode == StatusCodes.Status201Created)
                return Created(result.CreatedLocation ?? string.Empty, result.Body);

            return StatusCode(result.StatusCode, result.Body);
        }
    }
}