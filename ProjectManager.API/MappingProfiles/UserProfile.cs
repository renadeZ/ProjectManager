using Mapster;
using ProjectManager.API.DTOs;
using ProjectManager.API.Models;

namespace ProjectManager.API.MappingProfiles;

public class UserProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterDto, User>()
            .Map(dest => dest.UserName, src => src.Email);

        config.NewConfig<User, UserDto>()
            .Map(dest => dest.TeamName, src => src.Team != null ? src.Team.Name : string.Empty);
    }
}