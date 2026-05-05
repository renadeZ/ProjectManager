using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProjectManager.API.DTOs;
using ProjectManager.API.Services;

namespace ProjectManager.API.Controller;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class JobController : ControllerBase
{
    private readonly IJobService _jobService;

    public JobController(IJobService jobService)
    {
        _jobService = jobService;
    }

    [HttpPost]
    public async Task<IActionResult> AddJob([FromBody] JobDto job)
    {
        var result = await _jobService.AddJobAsync(job);
        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetJobById), new { id = result.Data?.Id }, result.Data);
        }
        return BadRequest(result.Message);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserJobs(string userId)
    {
        var result = await _jobService.GetUserJobsAsync(userId);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return BadRequest(result.Message);
    }

    [HttpGet("team/{teamId}")]
    public async Task<IActionResult> GetTeamJobs(int teamId)
    {
        var result = await _jobService.GetTeamJobsAsync(teamId);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return BadRequest(result.Message);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetJobById(int id)
    {
        var result = await _jobService.GetJobByIdAsync(id);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        if (result.Message == "Job not found")
        {
            return NotFound(result.Message);
        }
        return BadRequest(result.Message);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateJob(int id, [FromBody] JobDto job)
    {
        if (id != job.Id)
        {
            return BadRequest("ID in URL does not match ID in body.");
        }

        var result = await _jobService.UpdateJobAsync(job);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return BadRequest(result.Message);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJob(int id)
    {
        var result = await _jobService.DeleteJobAsync(id);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return BadRequest(result.Message);
    }
}