using Main.Models;
using Microsoft.EntityFrameworkCore;

namespace Main;

public class ScheduleDbContext : DbContext
{
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Lesson> Lessons { get; set; }

    public ScheduleDbContext(DbContextOptions<ScheduleDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lesson>()
            .HasOne(x => x.Teacher)
            .WithMany()
            .HasForeignKey(x => x.TeacherId);
    }
}