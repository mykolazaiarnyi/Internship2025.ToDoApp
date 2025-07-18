using Internship2025.ToDoApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Internship2025.ToDoApp.Data;

public class ToDoAppDbContext : DbContext
{
    public const string SeededUserId = "1f0df12a-929d-4a7e-ab5d-56a3c4540f90";

    public DbSet<ToDoItem> ToDoItems { get; set; }

    public DbSet<User> Users { get; set; }

    public ToDoAppDbContext(DbContextOptions<ToDoAppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ToDoItem>()
            .HasOne(t => t.User)
            .WithMany(u => u.ToDoItems)
            .HasForeignKey(t => t.UserId);

        modelBuilder.Entity<ToDoItem>()
            .Property(t => t.Description)
            .HasMaxLength(100);

        modelBuilder.Entity<User>()
            .Property(u => u.Name)
            .HasMaxLength(50);

        modelBuilder.Entity<User>()
            .HasData(new User { Id = SeededUserId, Name = "John Doe" });
    }
}
