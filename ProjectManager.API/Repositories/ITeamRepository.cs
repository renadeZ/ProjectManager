using ProjectManager.API.Models;

namespace ProjectManager.API.Repositories;

public interface ITeamRepository
{
    // Create
    Task<Team> AddTeamAsync(Team team);
    
    // Read 
    Task<List<Team>> GetAllTeamsAsync();
    Task<Team?> GetTeamByIdAsync(int id);
    Task<List<User>> GetTeamMembersAsync(int id);
    
    // Update
    Task<Team> UpdateTeamAsync(Team team);
    
    // Delete
    Task DeleteTeamAsync(int id);
}