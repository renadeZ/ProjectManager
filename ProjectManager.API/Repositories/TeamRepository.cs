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

    public async Task<Team> AddTeamAsync(Team team)
    {
        _context.Teams.Add(team);
        await _context.SaveChangesAsync();
        return team;
    }

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

    public async Task<Team> UpdateTeamAsync(Team team)
    {
        _context.Teams.Update(team);
        await _context.SaveChangesAsync();
        return team;
    }

    public async Task<bool> AssignUserToTeamAsync(string userId, int teamId)
    {
        var user = await _context.Users.FindAsync(userId);
        var team = await _context.Teams.FindAsync(teamId);
        if (user == null || team == null) return false;
        user.TeamId = teamId;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task DeleteTeamAsync(int id)
    {
        var team = await _context.Teams.FindAsync(id);
        if (team != null)
        {
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
        }
    }
}