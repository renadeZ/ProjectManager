using ProjectManager.API.Models;
using ProjectManager.API.DTOs;

namespace ProjectManager.API.Repositories;

public interface IJobRepository
{
    // Create
    Task<Job> AddJobAsync(Job job);
    // Read
    Task<List<Job>> GetUserJobsAsync(string userId);
    Task<List<Job>> GetTeamJobsAsync(int teamId);
    Task<Job?> GetJobByIdAsync(int id);
    // Update
    Task UpdateJobAsync(Job job);
    // Delete
    Task DeleteJobAsync(int id);
}
