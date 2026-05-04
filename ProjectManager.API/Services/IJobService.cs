using ProjectManager.API.DTOs;
using ProjectManager.API.Models;

namespace ProjectManager.API.Services;

public interface IJobService
{
    // Create
    Task<ServiceResult<JobDto>> AddJobAsync(JobDto job);
    // Read
    Task<ServiceResult<List<JobDto>>> GetAllJobsAsync(string userId);
    Task<ServiceResult<JobDto>> GetJobByIdAsync(int id);
    // Update
    Task<ServiceResult<JobDto>> UpdateJobAsync(JobDto job);
    // Delete
    Task<ServiceResult<JobDto>> DeleteJobAsync(int id);
}