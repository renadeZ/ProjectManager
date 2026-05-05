using ProjectManager.API.DTOs;
using ProjectManager.API.Models;

namespace ProjectManager.API.Services;

public interface IJobService
{
    Task<ServiceResult<JobDto>> AddJobAsync(JobDto job);
    Task<ServiceResult<List<JobDto>>> GetUserJobsAsync(string userId);
    Task<ServiceResult<List<JobDto>>> GetTeamJobsAsync(int teamId);
    Task<ServiceResult<JobDto>> GetJobByIdAsync(int id);
    Task<ServiceResult<JobDto>> UpdateJobAsync(JobDto job);
    Task<ServiceResult<JobDto>> DeleteJobAsync(int id);
}