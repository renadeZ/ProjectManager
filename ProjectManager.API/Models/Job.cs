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
        public User User { get; set; } = null!;
        public Team Team { get; set; } = null!;
    }
}