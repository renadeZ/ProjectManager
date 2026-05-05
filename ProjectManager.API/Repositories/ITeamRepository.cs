using ProjectManager.API.Models;

namespace ProjectManager.API.Repositories;

public interface ITeamRepository
{
    Task<Team> AddTeamAsync(Team team);
    Task<List<Team>> GetAllTeamsAsync();
    Task<Team?> GetTeamByIdAsync(int id);
    Task<List<User>> GetTeamMembersAsync(int id);
    Task<Team> UpdateTeamAsync(Team team);
    Task<bool> AssignUserToTeamAsync(string userId, int teamId);
    Task DeleteTeamAsync(int id);
}