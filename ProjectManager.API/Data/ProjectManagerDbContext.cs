using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectManager.API.Models;

namespace ProjectManager.API.Data;

public class ProjectManagerDbContext : IdentityDbContext<User>
{
    public DbSet<Team> Teams { get; set; }
    public DbSet<Job> Jobs { get; set; }

    public ProjectManagerDbContext(DbContextOptions<ProjectManagerDbContext> options) : base(options){}

    public ProjectManagerDbContext(){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); 

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasOne(j => j.User)
                .WithMany(u => u.Jobs)
                .HasForeignKey(j => j.AssignedUserId)
                .OnDelete(DeleteBehavior.SetNull);    

            entity.HasOne(j => j.Team)
                .WithMany(t => t.Jobs)
                .HasForeignKey(j => j.AssignedTeamId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasMany(t => t.Members)
                .WithOne(u => u.Team)
                .HasForeignKey(u => u.TeamId)
                .OnDelete(DeleteBehavior.SetNull); 
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=project_manager_v2.db");
        }
    }
}
