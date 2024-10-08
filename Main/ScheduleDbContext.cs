using Main.Models;
using Microsoft.EntityFrameworkCore;

namespace Main;

public class ScheduleDbContext : DbContext
{
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Lesson> Lessons { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL(
            $"Server={Environment.GetEnvironmentVariable("DB_SERVER")};" +
            $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
            $"Uid={Environment.GetEnvironmentVariable("DB_USER")};" +
            $"Pwd={Environment.GetEnvironmentVariable("DB_PASS")};"
            );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lesson>()
            .HasOne(x => x.Teacher)
            .WithMany()
            .HasForeignKey(x => x.TeacherId);
    }
}