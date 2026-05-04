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

    //Create
    public async Task<Job> AddJobAsync(Job job)
    {
        // job.DueDate = DateTime.Today.Add(deadlineTime.ToTimeSpan());
        _context.Jobs.Add(job);
        await _context.SaveChangesAsync();
        return job;
    }

    //Read
    public async Task<List<Job>> GetAllJobsAsync(string userId)
    {
        return await _context.Jobs
            .Include(j => j.User)
            .Include(j => j.Team)
            .Where(j => j.User.Id == userId)
            .ToListAsync();
    }

    public async Task<Job?> GetJobByIdAsync(int id)
    {
        return await _context.Jobs
            .Include(j => j.User)
            .Include(j => j.Team)
            .FirstOrDefaultAsync(j => j.Id == id);
    }

    //Update
    public async Task UpdateJobAsync(Job job)
    {
        _context.Jobs.Update(job);
        await _context.SaveChangesAsync();
    }

    //Delete
    public async Task DeleteJobAsync(int id)
    {
        var job = await GetJobByIdAsync(id);
        if (job != null)
        {
            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
        }
    }
}
