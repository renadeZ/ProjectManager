using ProjectManager.API.Data;
using ProjectManager.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ProjectManager.API.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly ProjectManagerDbContext _context;

    public TeamRepository(ProjectManagerDbContext context)
    {
        _context = context;
    }

    // Create
    public async Task<Team> AddTeamAsync(Team team)
    {
        _context.Teams.Add(team);
        await _context.SaveChangesAsync();
        return team;
    }

    // Read
    public async Task<List<Team>> GetAllTeamsAsync()
    {
        return await _context.Teams.Include(t => t.Members).ToListAsync();
    }

    public async Task<Team?> GetTeamByIdAsync(int id)
    {
        return await _context.Teams.Include(t => t.Members).FirstOrDefaultAsync(t => t.Id == id);
    }
    
    public async Task<List<User>> GetTeamMembersAsync(int id)
    {
        var team = await _context.Teams.Include(t => t.Members).FirstOrDefaultAsync(t => t.Id == id);
        return team?.Members ?? new List<User>();
    }

    // Update
    public async Task<Team> UpdateTeamAsync(Team team)
    {
        _context.Teams.Update(team);
        await _context.SaveChangesAsync();
        return team;
    }

    // Delete
    public async Task DeleteTeamAsync(int id)
    {
        var team = await GetTeamByIdAsync(id);
        if (team != null)
        {
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
        }
    }
}