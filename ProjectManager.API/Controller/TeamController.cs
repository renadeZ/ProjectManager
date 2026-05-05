using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProjectManager.API.DTOs;
using ProjectManager.API.Services;

namespace ProjectManager.API.Controller;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;

    public TeamController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpPost]
    public async Task<IActionResult> AddTeam([FromBody] TeamDto teamDto)
    {
        var result = await _teamService.AddTeamAsync(teamDto);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }

        return CreatedAtAction(nameof(GetTeamById), new { id = result.Data?.Id }, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTeams()
    {
        var result = await _teamService.GetAllTeamsAsync();
        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTeamById(int id)
    {
        var result = await _teamService.GetTeamByIdAsync(id);
        if (!result.IsSuccess)
        {
            if (result.Message == "Team not found")
            {
                return NotFound(result.Message);
            }
            return BadRequest(result.Message);
        }

        return Ok(result);
    }

    [HttpGet("{id}/members")]
    public async Task<IActionResult> GetTeamMembers(int id)
    {
        var result = await _teamService.GetTeamMembersAsync(id);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }

        return Ok(result);
    }

    [HttpPost("{id}/assign")]
    public async Task<IActionResult> AssignUserToTeam(int id, [FromQuery] string userId)
    {
        var result = await _teamService.AssignUserToTeamAsync(userId, id);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTeam(int id, [FromBody] TeamDto teamDto)
    {
        var result = await _teamService.UpdateTeamAsync(id, teamDto);
        if (!result.IsSuccess)
        {
            if (result.Message == "Team not found")
            {
                return NotFound(result.Message);
            }
            return BadRequest(result.Message);
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTeam(int id)
    {
        var result = await _teamService.DeleteTeamAsync(id);
        if (!result.IsSuccess)
        {
            if (result.Message == "Team not found")
            {
                return NotFound(result.Message);
            }
            return BadRequest(result.Message);
        }

        return Ok(result);
    }
}