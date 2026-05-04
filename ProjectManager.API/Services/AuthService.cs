using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
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
                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return ServiceResult<AuthResponseDto>.Failure("Registration failed");
                }

                // Add user to default role
                await _userManager.AddToRoleAsync(user, "User");

                var authResponse = await GenerateJwtToken(user);
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
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    return ServiceResult<AuthResponseDto>.Failure("Invalid email or password");
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
                if (!result.Succeeded)
                {
                    return ServiceResult<AuthResponseDto>.Failure("Invalid email or password");
                }

                var authResponse = await GenerateJwtToken(user);
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
                var user = await _userManager.FindByIdAsync(userId);
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

        private async Task<AuthResponseDto> GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"] ?? "your-super-secret-key-that-is-at-least-256-bits-long");

            var roles = await _userManager.GetRolesAsync(user);

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
            userDto.Roles = roles.ToList();

            return new AuthResponseDto
            {
                Token = tokenHandler.WriteToken(token),
                Expiration = tokenDescriptor.Expires.Value,
                User = userDto
            };
        }
    }
}