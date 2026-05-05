namespace ProjectManager.API.DTOs;

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new List<string>();
    public int? TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
}