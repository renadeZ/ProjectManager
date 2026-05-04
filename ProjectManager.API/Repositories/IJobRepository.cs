using ProjectManager.API.Models;
using ProjectManager.API.DTOs;

namespace ProjectManager.API.Repositories;

public interface IJobRepository
{
    // Create
    Task<Job> AddJobAsync(Job job);
    // Read
    Task<List<Job>> GetAllJobsAsync(string userId);
    Task<Job?> GetJobByIdAsync(int id);
    // Update
    Task UpdateJobAsync(Job job);
    // Delete
    Task DeleteJobAsync(int id);
}
