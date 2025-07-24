using Internship2025.ToDoApp.Data;
using Internship2025.ToDoApp.Data.Models;
using Internship2025.ToDoApp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Internship2025.ToDoApp.Tests;

public class DeleteToDoItemTests
{
    [Fact]
    public async Task ShouldDeleteItem_WhenExists()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var context = new ToDoAppDbContext(new DbContextOptionsBuilder<ToDoAppDbContext>().UseInMemoryDatabase("tests").Options);
        var currentUserService = new Mock<ICurrentUserService>();
        var service = new ToDoItemsService(context, currentUserService.Object);

        context.Users.Add(new User
        {
            Id = userId,
            UserName = "Test User"
        });

        var item = new ToDoItem
        {
            Description = "Test Item",
            UserId = userId
        };
        context.ToDoItems.Add(item);
        await context.SaveChangesAsync();

        currentUserService.Setup(x => x.GetUserId()).Returns(userId);

        // Act
        await service.DeleteToDoItemAsync(item.Id);
        
        // Assert
        var deletedItem = await context.ToDoItems.FindAsync(item.Id);
        Assert.Null(deletedItem); // Verify that the item was deleted
    }
}
