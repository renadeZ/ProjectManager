using ProjectManager.API.DTOs;

namespace ProjectManager.API.Services;

public interface ITeamService
{
    // Create
    Task<ServiceResult<TeamDto>> AddTeamAsync(TeamDto teamDto);
    
    // Read
    Task<ServiceResult<List<TeamDto>>> GetAllTeamsAsync();
    Task<ServiceResult<TeamDto>> GetTeamByIdAsync(int id);
    Task<ServiceResult<List<UserDto>>> GetTeamMembersAsync(int id);
    
    // Update
    Task<ServiceResult<TeamDto>> UpdateTeamAsync(int id, TeamDto teamDto);
    Task<ServiceResult<bool>> AssignUserToTeamAsync(string userId, int teamId);
    
    // Delete
    Task<ServiceResult<TeamDto>> DeleteTeamAsync(int id);
}
