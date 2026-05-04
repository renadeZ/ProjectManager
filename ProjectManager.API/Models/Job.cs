namespace ProjectManager.API.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public DateTime DueDate { get; set; } = DateTime.UtcNow;
        public User User { get; set; } = null!;
        public Team Team { get; set; } = null!;
    }
}