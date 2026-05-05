using ProjectManager.API.Data;
using ProjectManager.API.Models;
using ProjectManager.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ProjectManager.API.Repositories;

public class JobRepository : IJobRepository
{
    private readonly ProjectManagerDbContext _context;

    public JobRepository(ProjectManagerDbContext context)
    {
        _context = context;
    }

    public async Task<Job> AddJobAsync(Job job)
    {
        _context.Jobs.Add(job);
        await _context.SaveChangesAsync();
        return job;
    }

    public async Task<List<Job>> GetUserJobsAsync(string userId)
    {
        return await _context.Jobs
            .Include(j => j.User)
            .Include(j => j.Team)
            .Where(j => j.AssignedUserId == userId)
            .ToListAsync();
    }
    public async Task<List<Job>> GetTeamJobsAsync(int teamId)
    {
        return await _context.Jobs
            .Include(j => j.User)
            .Include(j => j.Team)
            .Where(j => j.AssignedTeamId == teamId)
            .ToListAsync();
    }

    public async Task<Job?> GetJobByIdAsync(int id)
    {
        return await _context.Jobs
            .Include(j => j.User)
            .Include(j => j.Team)
            .FirstOrDefaultAsync(j => j.Id == id);
    }

    public async Task UpdateJobAsync(Job job)
    {
        _context.Jobs.Update(job);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteJobAsync(int id)
    {
        var job = await _context.Jobs.FindAsync(id);
        if (job != null)
        {
            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
        }
    }
}