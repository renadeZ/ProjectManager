using ProjectManager.API.DTOs;

namespace ProjectManager.API.Services;
public interface IAuthService
{
    Task<ServiceResult<AuthResponseDto>> RegisterAsync(RegisterDto registerDto);
    Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto loginDto);
    Task<ServiceResult<UserDto>> GetCurrentUserAsync(string userId);
}