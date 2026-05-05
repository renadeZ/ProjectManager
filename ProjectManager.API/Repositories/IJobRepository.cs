using ProjectManager.API.Models;
using ProjectManager.API.DTOs;

namespace ProjectManager.API.Repositories;

public interface IJobRepository
{
    Task<Job> AddJobAsync(Job job);
    Task<List<Job>> GetUserJobsAsync(string userId);
    Task<List<Job>> GetTeamJobsAsync(int teamId);
    Task<Job?> GetJobByIdAsync(int id);
    Task UpdateJobAsync(Job job);
    Task DeleteJobAsync(int id);
}