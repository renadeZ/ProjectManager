namespace ProjectManager.API.DTOs;

public class JobDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime DueDate { get; set; } = DateTime.UtcNow;

    public string? AssignedUserId { get; set; }
    public int? AssignedTeamId { get; set; }
}