using Mapster;
using ProjectManager.API.DTOs;
using ProjectManager.API.Models;

namespace ProjectManager.API.MappingProfiles;

public class JobProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Job, JobDto>();
        config.NewConfig<JobDto, Job>();
    }
}