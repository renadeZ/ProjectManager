namespace ProjectManager.API.DTOs;

public class JobDto
{
    public string Title { get; set; } = string.Empty;
    public DateTime DueDate { get; set; } = DateTime.UtcNow;
}