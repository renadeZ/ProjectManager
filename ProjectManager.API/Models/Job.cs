using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManager.API.Models
{
    public class Job
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        public DateTime DueDate { get; set; } = DateTime.UtcNow;

        public string? AssignedUserId { get; set; }
        public User? User { get; set; }

        public int? AssignedTeamId { get; set; }
        public Team? Team { get; set; }
    }
}