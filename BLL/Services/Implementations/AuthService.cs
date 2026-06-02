using System.Security.Cryptography;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using BLL.Dtos;
using BLL.Services.Abstractions;

namespace BLL.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly EmsContext _context;
        private readonly IEmailSender _emailSender;
        //private readonly ISystemClaimService _systemClaimService;
        private readonly IRoleService _roleService;
        private readonly ILogger<AuthService> _logger;
        private const string ADMIN = "ADMIN";
        private const string CEO = "CEO";

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration configuration,
            EmsContext context,
            IEmailSender emailSender,
            ILogger<AuthService> logger,
            IRoleService roleService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _emailSender = emailSender;
            _logger = logger;
            _roleService = roleService;
        }

        public async Task<AuthServiceResult> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return AuthServiceResult.Unauthorized("Invalid username or password");

            if (user.IsDeleted)
                return AuthServiceResult.Unauthorized("Your account has been disabled. Please contact support.");

            var userRoles = await _userManager.GetRolesAsync(user);

            var roleName = userRoles.FirstOrDefault();
            if (string.IsNullOrEmpty(roleName))
                return AuthServiceResult.Unauthorized("User has no role assigned");

            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                return AuthServiceResult.Unauthorized("Role not found");

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("SecurityStamp", user.SecurityStamp ?? "")
            };

            foreach (var rn in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, rn));

            var roleClaims = await _roleManager.GetClaimsAsync(role);
            authClaims.AddRange(roleClaims);

            var authSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
            );

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTimeHelper.GetEgyptNow().AddDays(7),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return AuthServiceResult.Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                username = user.UserName,
                roles = userRoles,
                permissions = authClaims.Where(c => c.Type.StartsWith("HasAccess")).Select(c => c.Type)
            });
        }

        public async Task<AuthServiceResult> RegisterAsync(RegisterFactoryModel model, CancellationToken ct)
        {
            Normalize(model);

            var pre = await EnsureIdentityPrechecksAsync(model, ct);
            if (!pre.ok)
                return pre.errorResult!;

            if (!ValidatePassword(model.Password, out var pwdErrors))
            {
                return AuthServiceResult.BadRequest(new
                {
                    Status = "Error",
                    Message = "Password validation failed.",
                    Errors = pwdErrors
                });
            }

            var (created, user, errors) = await CreateUserAsync(model, ct);
            if (!created)
            {
                return AuthServiceResult.BadRequest(new
                {
                    Status = "Error",
                    Message = "User creation failed.",
                    Errors = errors
                });
            }

            try
            {
                var roleAssign = await TryAssignRoleAsync(user, pre.role!, ct);
                if (!roleAssign.ok)
                    return await FailAndCleanupAsync(user, roleAssign.message!);

                await AssignClaimsForRoleAsync(user, pre.role!.Name!, ct);
                await SendWelcomeEmailBestEffortAsync(user, model, pre.role!.Name!, ct);

                return AuthServiceResult.Created(new
                {
                    Status = "Success",
                    Message = "User created successfully.",
                    Username = user.UserName,
                    user.Email,
                    user.PhoneNumber,
                    Role = pre.role!.Name
                });
            }
            catch (Exception ex)
            {
                return AuthServiceResult.ServerError(new
                {
                    Status = "Error",
                    ex.Message,
                    ex.StackTrace
                });
            }
        }

        public async Task<AuthServiceResult> MyProfileAsync(ClaimsPrincipal principal)
        {
            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId!);

            if (user == null)
                return AuthServiceResult.NotFound(new { Status = "404", Message = "User not found" });

            var roles = await _userManager.GetRolesAsync(user);
            var claims = await _userManager.GetClaimsAsync(user);

            var result = new
            {
                user.Id,
                Username = user.UserName,
                user.Email,
                user.PhoneNumber,
                RoleName = roles.FirstOrDefault(),
                Claims = claims.Select(c => new { c.Type, c.Value }),
                user.IsDeleted
            };

            return AuthServiceResult.Ok(result);
        }

        public async Task<AuthServiceResult> UpdateMyProfileAsync(
            ClaimsPrincipal principal,
            UpdateUserProfileDto model)
        {
            var currentUserId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(currentUserId))
                return AuthServiceResult.Unauthorized(new { Message = "Invalid token" });

            var user = await _userManager.FindByIdAsync(currentUserId);

            if (user == null)
                return AuthServiceResult.NotFound(new { Message = "User not found" });

            if (!string.IsNullOrWhiteSpace(model.Username))
                user.UserName = model.Username;

            if (!string.IsNullOrWhiteSpace(model.Email))
                user.Email = model.Email;

            if (!string.IsNullOrWhiteSpace(model.PhoneNumber))
                user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return AuthServiceResult.ServerError(new { Message = "User update failed", result.Errors });

            return AuthServiceResult.Ok(new { Message = "Profile updated successfully" });
        }

        public async Task<AuthServiceResult> ChangePasswordAsync(
            ClaimsPrincipal principal,
            ChangePwDto changePwDto)
        {
            var userName = principal.Identity?.Name;
            var user = await _userManager.FindByNameAsync(userName!);

            if (user == null)
                return AuthServiceResult.ServerError(new { Status = "Error", Message = $"Please Try again" });

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, changePwDto.OldPassword);

            if (!isPasswordCorrect)
                return AuthServiceResult.ServerError(new { Status = "Error", Message = $"Old Password Incorrect!" });

            if (string.Equals(changePwDto.OldPassword, changePwDto.NewPassword))
                return AuthServiceResult.ServerError(new { Status = "Error", Message = $"You cannot use the same old password!" });

            if (!string.Equals(changePwDto.NewPassword, changePwDto.ConfirmNewPassword))
                return AuthServiceResult.ServerError(new { Status = "Error", Message = $"New Password and Confirm New Password doesn't match!" });

            var result = await _userManager.ChangePasswordAsync(user, changePwDto.OldPassword, changePwDto.NewPassword);

            if (!result.Succeeded)
                return AuthServiceResult.ServerError(new { Status = "Error", Message = $"Failed Changing Password please try again!" });

            return AuthServiceResult.Ok(new { Status = "Succeeded", Message = $"Password Updated Successfully!" });
        }

        public async Task<AuthServiceResult> AddRoleAsync(AddRoleDTO model)
        {
            if (await _roleManager.RoleExistsAsync(model.RoleName))
                return AuthServiceResult.ServerError(new { Status = "Error", Message = "Role already exists!" });

            if (model.Claims != null && model.Claims.Any())
            {
                var invalidClaims = model.Claims
                    .Where(c => string.IsNullOrWhiteSpace(c.Type) || string.IsNullOrWhiteSpace(c.Value))
                    .ToList();

                if (invalidClaims.Any())
                {
                    return AuthServiceResult.BadRequest(new
                    {
                        Status = "Error",
                        Message = "Some claims are invalid",
                        InvalidClaims = invalidClaims
                    });
                }
            }

            var newRole = new ApplicationRole { Name = model.RoleName };
            var roleResult = await _roleManager.CreateAsync(newRole);

            if (!roleResult.Succeeded)
            {
                return AuthServiceResult.ServerError(new
                {
                    Status = "Error",
                    Message = "Failed to create role",
                    roleResult.Errors
                });
            }

            var role = await _roleManager.FindByNameAsync(model.RoleName);

            if (role != null && model.Claims != null && model.Claims.Any())
            {
                foreach (var claimDto in model.Claims)
                {
                    var claim = new Claim(claimDto.Type, claimDto.Value);
                    var claimsResult = await _roleManager.AddClaimAsync(role, claim);

                    if (!claimsResult.Succeeded)
                    {
                        return AuthServiceResult.ServerError(new
                        {
                            Status = "Error",
                            Message = "Failed to add claim",
                            claimsResult.Errors
                        });
                    }
                }
            }

            return AuthServiceResult.Ok(new { Status = "Success", Message = "Role created successfully!" });
        }

        public async Task<AuthServiceResult> GetRoleDetailsAsync(RoleFilterRequest? filter)
        {
            filter ??= new RoleFilterRequest();

            var rolesQuery = _roleManager.Roles.AsQueryable().AsNoTracking();

            if (!string.IsNullOrEmpty(filter.RoleName))
            {
                rolesQuery = rolesQuery
                    .Where(r => r.Name!.ToLower() == filter.RoleName.ToLower());
            }

            var roles = await rolesQuery.ToListAsync();

            if (!roles.Any())
                return AuthServiceResult.NotFound(new { Status = "Error", Message = "No roles found" });

            var result = new List<RoleDetailsDTO>();

            foreach (var role in roles)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(role);

                var claims = roleClaims
                    .Select(c => new ClaimDto { Type = c.Type, Value = c.Value })
                    .ToList();

                if (filter.ClaimValues is not null && filter.ClaimValues.Any())
                {
                    var normalizedRoleClaims = claims.Select(c => c.Value.ToLower()).ToList();
                    var normalizedFilters = filter.ClaimValues.Select(cv => cv.ToLower()).ToList();

                    if (!normalizedFilters.Any(f => normalizedRoleClaims.Contains(f)))
                        continue;
                }

                result.Add(new RoleDetailsDTO
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    Claims = claims
                });
            }

            if (!result.Any())
                return AuthServiceResult.NotFound(new { Status = "Error", Message = "No roles match the filters" });

            return AuthServiceResult.Ok(result);
        }

        public async Task<AuthServiceResult> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles
                .AsNoTracking()
                .ToListAsync();

            if (!roles.Any())
                return AuthServiceResult.NotFound(new { Status = "Error", Message = "No roles found" });

            var result = new List<RoleDetailsDTO>();

            foreach (var role in roles)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(role);

                var claims = roleClaims
                    .Select(c => new ClaimDto { Type = c.Type, Value = c.Value })
                    .ToList();

                result.Add(new RoleDetailsDTO
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    Claims = claims
                });
            }

            return AuthServiceResult.Ok(result);
        }

        public async Task<AuthServiceResult> UpdateRoleAsync(UpdateRoleDto model)
        {
            try
            {
                var role = await _roleManager.FindByNameAsync(model.RoleName);

                if (role == null)
                    return AuthServiceResult.BadRequest(new { Status = "Error", Message = "Role does not exist!" });

                if (model.Claims != null && model.Claims.Any())
                {
                    var duplicateClaim = model.Claims
                        .GroupBy(c => new { c.Type, c.Value })
                        .FirstOrDefault(g => g.Count() > 1);

                    if (duplicateClaim != null)
                    {
                        return AuthServiceResult.BadRequest(new
                        {
                            Status = "Error",
                            Message = $"Duplicate claim in request: {duplicateClaim.Key.Type} - {duplicateClaim.Key.Value}"
                        });
                    }

                    var oldClaims = await _roleManager.GetClaimsAsync(role);

                    foreach (var claim in oldClaims)
                        await _roleManager.RemoveClaimAsync(role, claim);

                    foreach (var claimDto in model.Claims)
                    {
                        if (string.IsNullOrWhiteSpace(claimDto.Type) || string.IsNullOrWhiteSpace(claimDto.Value))
                            return AuthServiceResult.BadRequest(new { Status = "Error", Message = "Claim Type and Value are required." });

                        var result = await _roleManager.AddClaimAsync(role, new Claim(claimDto.Type, claimDto.Value));

                        if (!result.Succeeded)
                            return AuthServiceResult.BadRequest(new { Status = "Error", Message = "Failed to add claim", result.Errors });
                    }
                }

                return AuthServiceResult.Ok(new { Status = "Succeeded", Message = "Role updated successfully" });
            }
            catch (Exception ex)
            {
                return AuthServiceResult.ServerError(new
                {
                    success = false,
                    statusCode = 500,
                    message = "Unexpected server error!",
                    data = (object?)null,
                    errors = new { exception = ex.Message }
                });
            }
        }

        public async Task<AuthServiceResult> DeleteRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
                return AuthServiceResult.NotFound(new { Status = "Error", Message = "Role not found" });

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                return AuthServiceResult.ServerError(new
                {
                    Status = "Error",
                    Message = "Failed to delete role",
                    result.Errors
                });
            }

            return AuthServiceResult.Ok(new { Status = "Succeeded", Message = "Role deleted successfully" });
        }

        public async Task<AuthServiceResult> GetAllUsersWithRolesAsync(string? role, string? email)
        {
            var query =
                from u in _context.Users
                join ur in _context.UserRoles on u.Id equals ur.UserId
                join r in _context.Roles on ur.RoleId equals r.Id
                select new
                {
                    u.Id,
                    u.UserName,
                    u.Email,
                    RoleName = r.Name,
                    u.PhoneNumber,
                    u.IsDeleted
                };

            if (!string.IsNullOrEmpty(role))
            {
                query = query.Where(x => x.RoleName != null &&
                                         x.RoleName.ToLower() == role.ToLower());
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(x => x.Email != null &&
                                         x.Email.ToLower().Contains(email.ToLower()));
            }

            var result = await query.ToListAsync();

            return AuthServiceResult.Ok(result);
        }

        public async Task<AuthServiceResult> UpdateUserByAdminAsync(UpdateUserDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
                return AuthServiceResult.NotFound(new { Message = "User not found" });

            if (!string.IsNullOrEmpty(model.Role))
            {
                var oldRoles = await _userManager.GetRolesAsync(user);

                await _userManager.RemoveFromRolesAsync(user, oldRoles);
                await _userManager.AddToRoleAsync(user, model.Role);
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return AuthServiceResult.ServerError(new { Message = "Failed to update user", result.Errors });

            await _userManager.UpdateSecurityStampAsync(user);

            return AuthServiceResult.Ok(new { Message = "User role updated successfully" });
        }

        public async Task<AuthServiceResult> DeleteUserAsync(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.Equals(userId));

            if (user == null)
                return AuthServiceResult.NotFound(new { Status = "404", Message = "User not found" });

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return AuthServiceResult.Ok(new { Status = "200", Message = "User deleted successfully" });
        }

        public async Task<AuthServiceResult> ForgotPasswordAsync(ForgotPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return AuthServiceResult.Ok("Check your email..");

            var otpEntry = await _context.PasswordResetOtps
                .FirstOrDefaultAsync(o => o.Email == model.Email);

            //if (otpEntry != null)
            //{
            //    if (otpEntry.LockedUntil.HasValue && otpEntry.LockedUntil > DateTime.UtcNow)
            //    {
            //        var remaining = otpEntry.LockedUntil.Value - DateTime.UtcNow;
            //        return AuthServiceResult.BadRequest($"Too many attempts. Please try again after {remaining.Minutes} minutes.");
            //    }

            //    if ((DateTime.UtcNow - otpEntry.LastAttempt).TotalSeconds < 60)
            //    {
            //        //var secondsLeft = 60 - (DateTime.UtcNow - otpEntry.LastAttempt).TotalSeconds;
            //        return AuthServiceResult.BadRequest($"Please wait Math.Ceiling(secondsLeft) seconds before requesting another OTP.");
            //    }

            //    otpEntry.RequestCount++;
            //    otpEntry.LastAttempt = DateTimeHelper.GetEgyptNow();

            //    if (otpEntry.RequestCount >= 5)
            //    {
            //        otpEntry.LockedUntil = DateTime.UtcNow.AddMinutes(5);
            //        await _context.SaveChangesAsync();

            //        return AuthServiceResult.BadRequest("Too many attempts. You are blocked for 5 minutes.");
            //    }

            //    _context.PasswordResetOtps.Remove(otpEntry);
            //}

            var bytes = RandomNumberGenerator.GetBytes(6);
            var otp = string.Concat(bytes.Select(b => (b % 10).ToString()));

            var newOtp = new PasswordResetOtp
            {
                Email = model.Email,
                Code = otp,
                ExpirationTime = DateTime.UtcNow.AddMinutes(5),
                CreatedAt = DateTimeHelper.GetEgyptNow(),
                RequestCount = otpEntry?.RequestCount ?? 1,
                LastAttempt = DateTimeHelper.GetEgyptNow()
            };

            await _context.PasswordResetOtps.AddAsync(newOtp);
            await _context.SaveChangesAsync();

            await _emailSender.SendOtpEmail(model.Email, otp);

            return AuthServiceResult.Ok("OTP sent successfully.");
        }

        public async Task<AuthServiceResult> ValidateOtpAsync(VerifyOtpDto model)
        {
            var otpEntry = await _context.PasswordResetOtps
                .FirstOrDefaultAsync(o => o.Email == model.Email && o.Code == model.OTP);

            if (otpEntry == null || otpEntry.ExpirationTime < DateTime.UtcNow)
                return AuthServiceResult.BadRequest("Invalid or expired OTP.");

            otpEntry.IsVerified = true;
            await _context.SaveChangesAsync();

            return AuthServiceResult.Ok("OTP verified successfully.");
        }

        public async Task<AuthServiceResult> ResetPasswordAsync(ResetPasswordDto model)
        {
            //var otpEntry = await _context.PasswordResetOtps
            //    .FirstOrDefaultAsync(o => o.Email == model.Email);
            var otpEntry = await _context.PasswordResetOtps
            .Where(o => o.Email == model.Email && o.IsVerified)
            .OrderByDescending(o => o.ExpirationTime)
            .FirstOrDefaultAsync();

            if (otpEntry == null)
                return AuthServiceResult.BadRequest("Invalid request.");

            if (!otpEntry.IsVerified)
                return AuthServiceResult.BadRequest("OTP not verified.");

            if (otpEntry.ExpirationTime < DateTime.UtcNow)
                return AuthServiceResult.BadRequest("OTP expired.");

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return AuthServiceResult.BadRequest("Invalid request.");

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);

            if (!result.Succeeded)
                return AuthServiceResult.BadRequest(new { Errors = result.Errors.Select(e => e.Description) });

            _context.PasswordResetOtps.Remove(otpEntry);
            await _context.SaveChangesAsync();

            return AuthServiceResult.Ok("Password reset successfully.");
        }

        public async Task<AuthServiceResult> AddClaimAsync(List<AddClaimDto> model)
        {
            if (model == null || !model.Any())
            {
                return AuthServiceResult.BadRequest(new
                {
                    Status = "Error",
                    Message = "Claims list cannot be empty."
                });
            }

            var addedClaims = new List<object>();
            var existingClaims = new List<string>();

            foreach (var item in model)
            {
                if (string.IsNullOrWhiteSpace(item.ClaimName))
                    continue;

                var existingClaim = await _context.RoleClaims
                         .FirstOrDefaultAsync(x => x.ClaimType == item.ClaimName);

                if (existingClaim != null)
                {
                    existingClaims.Add(item.ClaimName);
                    continue;
                }

                _context.RoleClaims.Add(new IdentityRoleClaim<string>
                {
                    ClaimType = item.ClaimName,
                    ClaimValue = item.ClaimName
                });

                addedClaims.Add(new { item.ClaimName });
            }

            return AuthServiceResult.Ok(new
            {
                Status = "Success",
                Added = addedClaims,
                AlreadyExists = existingClaims
            });
        }

        //public async Task<AuthServiceResult> GetAllSystemClaimsAsync(ClaimsPrincipal user)
        //{
        //    if (!user.HasClaim("Permission", "ManageRoles"))
        //    {
        //        return AuthServiceResult.Forbidden(new
        //        {
        //            Status = "Error",
        //            Message = "You don't have the required permission."
        //        });
        //    }

        //    //var claims = await _systemClaimService.GetAllClaimsAsync();

        //    if (!claims.Any())
        //        return AuthServiceResult.NotFound(new { Status = "Error", Message = "No claims found" });

        //    return AuthServiceResult.Ok(new
        //    {
        //        Status = "Success",
        //        Data = claims.Select(c => new { c.Id, c.ClaimName })
        //    });
        //}

        private static void Normalize(RegisterFactoryModel m)
        {
            m.Email = m.Email?.Trim();
            m.Username = m.Username?.Trim();
            m.RoleName = m.RoleName?.Trim();
        }

        private async Task<(bool ok, IdentityRole? role, AuthServiceResult? errorResult)>
            EnsureIdentityPrechecksAsync(RegisterFactoryModel model, CancellationToken ct)
        {
            if (await _userManager.FindByEmailAsync(model.Email!) is not null)
            {
                return (false, null, AuthServiceResult.Conflict(new
                {
                    Status = "Error",
                    Message = "User with this email already exists."
                }));
            }

            if (await _userManager.FindByNameAsync(model.Username!) is not null)
            {
                return (false, null, AuthServiceResult.Conflict(new
                {
                    Status = "Error",
                    Message = "Username already exists."
                }));
            }

            var role = await _roleManager.FindByNameAsync(model.RoleName!);

            if (role is null)
            {
                return (false, null, AuthServiceResult.BadRequest(new
                {
                    Status = "Error",
                    Message = $"Role '{model.RoleName}' does not exist."
                }));
            }

            return (true, role, null);
        }

        private async Task<(bool succeeded, ApplicationUser user, IEnumerable<string> errors)>
            CreateUserAsync(RegisterFactoryModel model, CancellationToken ct)
        {
            var user = new ApplicationUser
            {
                Email = model.Email!,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username!,
                PhoneNumber = model.PhoneNumber,
                CreatedAt = DateTimeHelper.GetEgyptNow()
            };

            var res = await _userManager.CreateAsync(user, model.Password);

            return res.Succeeded
                ? (true, user, Enumerable.Empty<string>())
                : (false, user, res.Errors.Select(e => e.Description));
        }

        private async Task<(bool ok, string? message)>
            TryAssignRoleAsync(ApplicationUser user, IdentityRole role, CancellationToken ct)
        {
            var res = await _userManager.AddToRoleAsync(user, role.Name!);

            if (!res.Succeeded)
                return (false, string.Join("; ", res.Errors.Select(e => e.Description)));

            return (true, null);
        }

        private bool IsAdminRole(string roleName)
        {
            return roleName.Equals(CEO, StringComparison.OrdinalIgnoreCase) ||
                   roleName.Equals(ADMIN, StringComparison.OrdinalIgnoreCase);
        }

        private async Task AssignClaimsForRoleAsync(ApplicationUser user, string roleName, CancellationToken ct)
        {
            if (IsAdminRole(roleName))
            {
                await _userManager.AddClaimAsync(user, new Claim("permission", "HasAccessToEverything"));
                return;
            }

            var roleClaims = await _roleService.GetRoleClaimsAsync(roleName);

            if (roleClaims is not null && roleClaims.Any())
                await _userManager.AddClaimsAsync(user, roleClaims);
        }

        private async Task SendWelcomeEmailBestEffortAsync(
            ApplicationUser user,
            RegisterFactoryModel model,
            string roleName,
            CancellationToken ct)
        {
            var dto = new UserWelcomeDTO
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber!,
                RoleName = roleName,
                Password = model.Password
            };

            await _emailSender.SendWelcomeEmailAsync(dto);
        }

        private async Task<AuthServiceResult> FailAndCleanupAsync(
            ApplicationUser user,
            string message,
            Exception? ex = null)
        {
            try
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Any())
                    await _userManager.RemoveFromRolesAsync(user, roles);

                var claims = await _userManager.GetClaimsAsync(user);
                if (claims.Any())
                    await _userManager.RemoveClaimsAsync(user, claims);

                await _userManager.DeleteAsync(user);
            }
            catch (Exception cleanupEx)
            {
                _logger.LogError(cleanupEx, "Cleanup failed after error: {Message}", message);
            }

            if (ex is not null)
                _logger.LogError(ex, "Register failed: {Message}", message);

            return AuthServiceResult.ServerError(new { Status = "Error", Message = message });
        }

        private bool ValidatePassword(string password, out List<string> errors)
        {
            errors = new List<string>();

            if (string.IsNullOrWhiteSpace(password))
                errors.Add("Password cannot be empty.");

            if (password.Length < 8)
                errors.Add("Password must be at least 8 characters long.");

            if (!password.Any(char.IsUpper))
                errors.Add("Password must contain at least one uppercase letter.");

            if (!password.Any(char.IsLower))
                errors.Add("Password must contain at least one lowercase letter.");

            if (!password.Any(char.IsDigit))
                errors.Add("Password must contain at least one number.");

            if (!password.Any(ch => "!@#$%^&*()_-+=<>?/{}~|".Contains(ch)))
                errors.Add("Password must contain at least one special character.");

            return errors.Count == 0;
        }
    }
}
