using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectManager.API.Models;

namespace ProjectManager.API.Data;

public class ProjectManagerDbContext : IdentityDbContext<User>
{
    // public DbSet<User> Users { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Job> Jobs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); 

        modelBuilder.Entity<User>(entity =>
        {

        });

        modelBuilder.Entity<Job>(entity =>
        {
            // Job Many-to-One User
            entity.HasOne(j => j.User)
                .WithMany(u => u.Jobs)
                .HasForeignKey("AssignedUserId")
                .OnDelete(DeleteBehavior.SetNull);    

            // Job Many-to-One Team
            entity.HasOne(j => j.Team)
                .WithMany(j => j.Jobs)
                .HasForeignKey("AssignedTeamId")
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Team>(entity =>
        {
            // Team One-to-Many User
            entity.HasMany(t => t.Members)
                .WithOne(u => u.Team)
                .OnDelete(DeleteBehavior.SetNull); 
        });
            
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=project_manager.db");
    }

    public ProjectManagerDbContext(DbContextOptions<ProjectManagerDbContext> options) : base(options){}

    public ProjectManagerDbContext(){}
}