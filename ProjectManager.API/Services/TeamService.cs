using ProjectManager.API.DTOs;
using ProjectManager.API.Models;
using ProjectManager.API.Repositories;
using Mapster;

namespace ProjectManager.API.Services;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _repository;

    public TeamService(ITeamRepository repository)
    {
        _repository = repository;
    }

    // Create
    public async Task<ServiceResult<TeamDto>> AddTeamAsync(TeamDto teamDto)
    {
        try
        {
            var team = teamDto.Adapt<Team>();
            Team createdTeam = await _repository.AddTeamAsync(team);
            return ServiceResult<TeamDto>.Success(createdTeam.Adapt<TeamDto>(), "Team created successfully");
        }
        catch (Exception ex)
        {
            return ServiceResult<TeamDto>.Failure($"Error creating team: {ex.Message}");
        }
    }

    // Read
    public async Task<ServiceResult<List<TeamDto>>> GetAllTeamsAsync()
    {
        try
        {
            List<Team> teams = await _repository.GetAllTeamsAsync();
            if (teams.Count == 0)
            {
                return ServiceResult<List<TeamDto>>.Failure("No teams found");
            }
            return ServiceResult<List<TeamDto>>.Success(teams.Adapt<List<TeamDto>>(), "Teams retrieved successfully");
        }
        catch (Exception ex)
        {
            return ServiceResult<List<TeamDto>>.Failure($"Error retrieving teams: {ex.Message}");
        }
    }

    public async Task<ServiceResult<TeamDto>> GetTeamByIdAsync(int id)
    {
        try
        {
            Team? team = await _repository.GetTeamByIdAsync(id);
            if (team == null)
            {
                return ServiceResult<TeamDto>.Failure("Team not found");
            }
            return ServiceResult<TeamDto>.Success(team.Adapt<TeamDto>(), "Team retrieved successfully");
        }
        catch (Exception ex)
        {
            return ServiceResult<TeamDto>.Failure($"Error retrieving team: {ex.Message}");
        }
    }

    public async Task<ServiceResult<List<UserDto>>> GetTeamMembersAsync(int id)
    {
        try
        {
            List<User> members = await _repository.GetTeamMembersAsync(id);
            if (members.Count == 0)
            {
                return ServiceResult<List<UserDto>>.Failure("No members found for this team");
            }
            
            return ServiceResult<List<UserDto>>.Success(members.Adapt<List<UserDto>>(), "Team members retrieved successfully");
        }
        catch (Exception ex)
        {
            return ServiceResult<List<UserDto>>.Failure($"Error retrieving team members: {ex.Message}");
        }
    }

    // Update
    public async Task<ServiceResult<TeamDto>> UpdateTeamAsync(int id, TeamDto teamDto)
    {
        try
        {
            var existingTeam = await _repository.GetTeamByIdAsync(id);
            if (existingTeam == null)
            {
                return ServiceResult<TeamDto>.Failure("Team not found");
            }
            existingTeam.Name = teamDto.Name;
            
            await _repository.UpdateTeamAsync(existingTeam);
            
            return ServiceResult<TeamDto>.Success(existingTeam.Adapt<TeamDto>(), "Team updated successfully");
        }
        catch (Exception ex)
        {
            return ServiceResult<TeamDto>.Failure($"Error updating team: {ex.Message}");
        }
    }

    public async Task<ServiceResult<bool>> AssignUserToTeamAsync(string userId, int teamId)
    {
        try
        {
            var success = await _repository.AssignUserToTeamAsync(userId, teamId);
            if (!success)
            {
                return ServiceResult<bool>.Failure("User or Team not found");
            }
            return ServiceResult<bool>.Success(true, "User assigned to team successfully");
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Failure($"Error assigning user to team: {ex.Message}");
        }
    }

    // Delete
    public async Task<ServiceResult<TeamDto>> DeleteTeamAsync(int id)
    {
        try
        {
            var existingTeam = await _repository.GetTeamByIdAsync(id);
            if (existingTeam == null)
            {
                return ServiceResult<TeamDto>.Failure("Team not found");
            }

            await _repository.DeleteTeamAsync(id);
            
            return ServiceResult<TeamDto>.Success(existingTeam.Adapt<TeamDto>(), "Team deleted successfully");
        }
        catch (Exception ex)
        {
            return ServiceResult<TeamDto>.Failure($"Error deleting team: {ex.Message}");
        }
    }
}
