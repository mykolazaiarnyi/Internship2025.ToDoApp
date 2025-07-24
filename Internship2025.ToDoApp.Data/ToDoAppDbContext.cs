using Internship2025.ToDoApp.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Internship2025.ToDoApp.Data;

public class ToDoAppDbContext : IdentityDbContext<User>
{
    public DbSet<ToDoItem> ToDoItems { get; set; }

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
    }
}
