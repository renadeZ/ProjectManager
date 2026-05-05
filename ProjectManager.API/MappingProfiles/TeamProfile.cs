using Mapster;
using ProjectManager.API.DTOs;
using ProjectManager.API.Models;

namespace ProjectManager.API.MappingProfiles;

public class TeamProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Team, TeamDto>();
        config.NewConfig<TeamDto, Team>();
    }
}