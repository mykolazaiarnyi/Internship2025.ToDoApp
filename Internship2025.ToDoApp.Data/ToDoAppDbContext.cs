using Internship2025.ToDoApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Internship2025.ToDoApp.Data;

public class ToDoAppDbContext : DbContext
{
    public DbSet<ToDoItem> ToDoItems { get; set; }

    public DbSet<User> Users { get; set; }

    //public ToDoAppDbContext(DbContextOptions<ToDoAppDbContext> options)
    //    : base(options)
    //{
    //}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        const string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ToDoAppDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        optionsBuilder.UseSqlServer(connectionString);

        base.OnConfiguring(optionsBuilder);
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
    }
}
