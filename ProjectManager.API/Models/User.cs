using Microsoft.AspNetCore.Identity;
namespace ProjectManager.API.Models;

public class User : IdentityUser
{
    public string Name { get; set; } = string.Empty;

    public List<Job> Jobs { get; set; } = new List<Job>();
    public Team Team { get; set; } = null!;

}
