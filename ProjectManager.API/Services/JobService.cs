using ProjectManager.API.DTOs;
using ProjectManager.API.Models;
using ProjectManager.API.Repositories;
using Mapster;

namespace ProjectManager.API.Services;

public class JobService : IJobService
{
    private readonly IJobRepository _repository;

    public JobService(IJobRepository repository)
    {
        _repository = repository;
    }

    // Create
    public async Task<ServiceResult<JobDto>> AddJobAsync(JobDto jobDto)
    {
        try
        {
            var job = jobDto.Adapt<Job>();
            Job createdJob = await _repository.AddJobAsync(job);
            return ServiceResult<JobDto>.Success(createdJob.Adapt<JobDto>(), "Job Created Succesfully");
        }
        catch (Exception ex)
        {
            return ServiceResult<JobDto>.Failure($"Error creating job: {ex.Message}");
        }
    }
    
    // Read
    public async Task<ServiceResult<List<JobDto>>> GetUserJobsAsync(string userId)
    {
        try
        {
            List<Job> jobs = await _repository.GetUserJobsAsync(userId);
            if (jobs.Count == 0)
            {
                return ServiceResult<List<JobDto>>.Failure($"Error retrieving jobs: No jobs found");
            }
            return ServiceResult<List<JobDto>>.Success(jobs.Adapt<List<JobDto>>(), "Jobs retrieved successfully");
        }
        catch (Exception ex)
        {
            return ServiceResult<List<JobDto>>.Failure($"Error retrieving jobs: {ex.Message}");
        }
    
    }     public async Task<ServiceResult<List<JobDto>>> GetTeamJobsAsync(int teamId)
    {
        try
        {
            List<Job> jobs = await _repository.GetTeamJobsAsync(teamId);
            if (jobs.Count == 0)
            {
                return ServiceResult<List<JobDto>>.Failure($"Error retrieving jobs: No jobs found");
            }
            return ServiceResult<List<JobDto>>.Success(jobs.Adapt<List<JobDto>>(), "Jobs retrieved successfully");
        }
        catch (Exception ex)
        {
            return ServiceResult<List<JobDto>>.Failure($"Error retrieving jobs: {ex.Message}");
        }
    }    
    
    public async Task<ServiceResult<JobDto>> GetJobByIdAsync(int id)
    {
        try
        {
            Job? job = await _repository.GetJobByIdAsync(id);
            if (job == null)
            {
                return ServiceResult<JobDto>.Failure($"Error retrieving jobs: No jobs found");
            }
            return ServiceResult<JobDto>.Success(job.Adapt<JobDto>(), "Jobs retrieved successfully");
        }
        catch (Exception ex)
        {
            return ServiceResult<JobDto>.Failure($"Error retrieving jobs: {ex.Message}");
        }
    }
    
    // Update
    public async Task<ServiceResult<JobDto>> UpdateJobAsync(JobDto jobDto)
    {
        try
        {
            var job = jobDto.Adapt<Job>();
            await _repository.UpdateJobAsync(job);
            return ServiceResult<JobDto>.Success(jobDto, "Job updated successfully");
        }
        catch (Exception ex)
        {
            return ServiceResult<JobDto>.Failure($"Error updating job: {ex.Message}");
        }
    }

    // Delete
    public async Task<ServiceResult<JobDto>> DeleteJobAsync(int id)
    {
        try
        {
            var existingJob = await _repository.GetJobByIdAsync(id);
            if (existingJob == null)
            {
                return ServiceResult<JobDto>.Failure("Job not found");
            }

            await _repository.DeleteJobAsync(id);
            return ServiceResult<JobDto>.Success(existingJob.Adapt<JobDto>(), "Job deleted successfully");
        }
        catch (Exception ex)
        {
            return ServiceResult<JobDto>.Failure($"Error deleting job: {ex.Message}");
        }
    }
}
