using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Mapster;
using ProjectManager.API.Models;
using ProjectManager.API.DTOs;

namespace ProjectManager.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<ServiceResult<AuthResponseDto>> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
                if (existingUser != null)
                {
                    return ServiceResult<AuthResponseDto>.Failure("User with this email already exists");
                }

                var user = registerDto.Adapt<User>();
                if (string.IsNullOrEmpty(user.UserName))
                {
                    user.UserName = registerDto.Email;
                }

                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return ServiceResult<AuthResponseDto>.Failure($"Registration failed: {errors}");
                }

                var role = string.IsNullOrWhiteSpace(registerDto.Role) ? "User" : registerDto.Role;
                var roles = new List<string> { role };
                try
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
                catch
                {
                    roles.Clear();
                    // If the specific role fails, try adding the default "User" role
                    if (role != "User")
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                        roles.Add("User");
                    }
                }

                var authResponse = await GenerateJwtToken(user, roles);
                return ServiceResult<AuthResponseDto>.Success(authResponse, "Registration successful");
            }
            catch (Exception ex)
            {
                return ServiceResult<AuthResponseDto>.Failure($"Registration error: {ex.Message}");
            }
        }

        public async Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var normalizedEmail = _userManager.NormalizeEmail(loginDto.Email);
                var user = await _userManager.Users
                    .Include(u => u.Team)
                    .FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);

                if (user == null)
                {
                    return ServiceResult<AuthResponseDto>.Failure("Invalid email or password");
                }

                var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (!isPasswordValid)
                {
                    return ServiceResult<AuthResponseDto>.Failure("Invalid email or password");
                }

                var roles = await _userManager.GetRolesAsync(user);
                var authResponse = await GenerateJwtToken(user, roles.ToList());
                return ServiceResult<AuthResponseDto>.Success(authResponse, "Login successful");
            }
            catch (Exception ex)
            {
                return ServiceResult<AuthResponseDto>.Failure($"Login error: {ex.Message}");
            }
        }

        public async Task<ServiceResult<UserDto>> GetCurrentUserAsync(string userId)
        {
            try
            {
                var user = await _userManager.Users.Include(u => u.Team).FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    return ServiceResult<UserDto>.Failure("User not found");
                }

                var userDto = user.Adapt<UserDto>();
                var roles = await _userManager.GetRolesAsync(user);
                userDto.Roles = roles.ToList();

                return ServiceResult<UserDto>.Success(userDto, "User retrieved successfully");
            }
            catch (Exception ex)
            {
                return ServiceResult<UserDto>.Failure($"Error retrieving user: {ex.Message}");
            }
        }

        private async Task<AuthResponseDto> GenerateJwtToken(User user, List<string>? roles = null)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secret = jwtSettings["Secret"] ?? "your-super-secret-key-that-is-at-least-256-bits-long";
            var key = Encoding.ASCII.GetBytes(secret);

            roles ??= (await _userManager.GetRolesAsync(user)).ToList();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim(ClaimTypes.Email, user.Email ?? "")
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var userDto = user.Adapt<UserDto>();
            userDto.Roles = roles;

            return new AuthResponseDto
            {
                Token = tokenHandler.WriteToken(token),
                Expiration = tokenDescriptor.Expires.Value,
                User = userDto
            };
        }
    }
}