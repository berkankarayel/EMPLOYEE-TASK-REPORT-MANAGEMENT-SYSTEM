using Microsoft.EntityFrameworkCore;
using EmployeeTaskManagement.Domain.Entities;

namespace EmployeeTaskManagement.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<TaskHistory> TaskHistories { get; set; }
    public DbSet<LeaveRequest> LeaveRequests { get; set; }
    public DbSet<SystemLog> SystemLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User - Task (1-N)
        modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.AssignedUser)
            .WithMany(u => u.Tasks)
            .HasForeignKey(t => t.AssignedUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Task - TaskHistory (1-N)
        modelBuilder.Entity<TaskHistory>()
            .HasOne(th => th.Task)
            .WithMany(t => t.TaskHistories)
            .HasForeignKey(th => th.TaskId);

        // User - LeaveRequest (1-N)
        modelBuilder.Entity<LeaveRequest>()
            .HasOne(lr => lr.User)
            .WithMany(u => u.LeaveRequests)
            .HasForeignKey(lr => lr.UserId);
    }
}
