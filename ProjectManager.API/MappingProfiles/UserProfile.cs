using ProjectManager.API.DTOs;
using ProjectManager.API.Models;
using Mapster;

namespace ProjectManager.API.MappingProfiles;

public class UserProfile
{
    public static void UserConfigure()
    {
        TypeAdapterConfig<User, UserDto>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Email, src => src.Email);

    }
}