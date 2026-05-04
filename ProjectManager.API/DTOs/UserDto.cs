namespace ProjectManager.API.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new List<string>();
    public List<string> JobTitles { get; set; } = new List<string>();
    public string TeamName { get; set; } = string.Empty;
}