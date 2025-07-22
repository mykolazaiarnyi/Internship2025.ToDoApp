using Internship2025.ToDoApp.Data;
using Internship2025.ToDoApp.Data.Models;
using Internship2025.ToDoApp.Domain.DTOs;
using Internship2025.ToDoApp.Domain.Exceptions;
using Internship2025.ToDoApp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Internship2025.ToDoApp.Tests;

public class ToDoItemsServiceTests
{
    private ToDoAppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ToDoAppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ToDoAppDbContext(options);
    }

    private Mock<ICurrentUserService> CreateUserService(string userId)
    {
        var mock = new Mock<ICurrentUserService>();
        mock.Setup(x => x.GetUserId()).Returns(userId);
        return mock;
    }

    [Fact]
    public async Task AddToDoItemAsync_ShouldAddItem()
    {
        var context = CreateDbContext();
        var userId = Guid.NewGuid().ToString();
        var currentUserService = CreateUserService(userId);
        var service = new ToDoItemsService(context, currentUserService.Object);

        context.Users.Add(new User { Id = userId, Name = "Test User" });
        await context.SaveChangesAsync();

        var dto = new CreateToDoItemDto
        {
            Description = "New Item",
            DueDate = DateTime.UtcNow.AddDays(1)
        };

        await service.AddToDoItemAsync(dto);

        var item = await context.ToDoItems.FirstOrDefaultAsync();
        Assert.NotNull(item);
        Assert.Equal(dto.Description, item.Description);
        Assert.Equal(dto.DueDate, item.DueDate);
        Assert.False(item.IsDone);
        Assert.Equal(userId, item.UserId);
    }

    [Fact]
    public async Task UpdateToDoItemAsync_ShouldUpdateItem_WhenOwned()
    {
        var context = CreateDbContext();
        var userId = Guid.NewGuid().ToString();
        var currentUserService = CreateUserService(userId);
        var service = new ToDoItemsService(context, currentUserService.Object);

        context.Users.Add(new User { Id = userId, Name = "Test User" });
        var item = new ToDoItem
        {
            Description = "Old",
            DueDate = DateTime.UtcNow,
            UserId = userId
        };
        context.ToDoItems.Add(item);
        await context.SaveChangesAsync();

        var dto = new UpdateToDoItemDto
        {
            Description = "Updated",
            DueDate = DateTime.UtcNow.AddDays(2)
        };

        await service.UpdateToDoItemAsync(item.Id, dto);

        var updated = await context.ToDoItems.FindAsync(item.Id);
        Assert.Equal(dto.Description, updated.Description);
        Assert.Equal(dto.DueDate, updated.DueDate);
    }

    [Fact]
    public async Task UpdateToDoItemAsync_ShouldThrow_WhenNotFound()
    {
        var context = CreateDbContext();
        var userId = Guid.NewGuid().ToString();
        var currentUserService = CreateUserService(userId);
        var service = new ToDoItemsService(context, currentUserService.Object);

        var dto = new UpdateToDoItemDto { Description = "X", DueDate = null };

        await Assert.ThrowsAsync<ItemNotFoundException>(() =>
            service.UpdateToDoItemAsync(999, dto));
    }

    [Fact]
    public async Task UpdateToDoItemAsync_ShouldThrow_WhenNotOwned()
    {
        var context = CreateDbContext();
        var userId = Guid.NewGuid().ToString();
        var otherUserId = Guid.NewGuid().ToString();
        var currentUserService = CreateUserService(userId);
        var service = new ToDoItemsService(context, currentUserService.Object);

        context.Users.Add(new User { Id = otherUserId, Name = "Other" });
        var item = new ToDoItem
        {
            Description = "Old",
            DueDate = DateTime.UtcNow,
            UserId = otherUserId
        };
        context.ToDoItems.Add(item);
        await context.SaveChangesAsync();

        var dto = new UpdateToDoItemDto { Description = "X", DueDate = null };

        await Assert.ThrowsAsync<UserDoesNotOwnItemException>(() =>
            service.UpdateToDoItemAsync(item.Id, dto));
    }

    [Fact]
    public async Task MarkAsDoneAsync_ShouldMarkDone_WhenOwnedAndNotDone()
    {
        var context = CreateDbContext();
        var userId = Guid.NewGuid().ToString();
        var currentUserService = CreateUserService(userId);
        var service = new ToDoItemsService(context, currentUserService.Object);

        context.Users.Add(new User { Id = userId, Name = "Test User" });
        var item = new ToDoItem
        {
            Description = "Test",
            IsDone = false,
            UserId = userId
        };
        context.ToDoItems.Add(item);
        await context.SaveChangesAsync();

        await service.MarkAsDoneAsync(item.Id);

        var updated = await context.ToDoItems.FindAsync(item.Id);
        Assert.True(updated.IsDone);
    }

    [Fact]
    public async Task MarkAsDoneAsync_ShouldThrow_WhenAlreadyDone()
    {
        var context = CreateDbContext();
        var userId = Guid.NewGuid().ToString();
        var currentUserService = CreateUserService(userId);
        var service = new ToDoItemsService(context, currentUserService.Object);

        context.Users.Add(new User { Id = userId, Name = "Test User" });
        var item = new ToDoItem
        {
            Description = "Test",
            IsDone = true,
            UserId = userId
        };
        context.ToDoItems.Add(item);
        await context.SaveChangesAsync();

        await Assert.ThrowsAsync<ItemAlreadyDoneException>(() =>
            service.MarkAsDoneAsync(item.Id));
    }

    [Fact]
    public async Task MarkAsDoneAsync_ShouldThrow_WhenNotOwned()
    {
        var context = CreateDbContext();
        var userId = Guid.NewGuid().ToString();
        var otherUserId = Guid.NewGuid().ToString();
        var currentUserService = CreateUserService(userId);
        var service = new ToDoItemsService(context, currentUserService.Object);

        context.Users.Add(new User { Id = otherUserId, Name = "Other" });
        var item = new ToDoItem
        {
            Description = "Test",
            IsDone = false,
            UserId = otherUserId
        };
        context.ToDoItems.Add(item);
        await context.SaveChangesAsync();

        await Assert.ThrowsAsync<UserDoesNotOwnItemException>(() =>
            service.MarkAsDoneAsync(item.Id));
    }

    [Fact]
    public async Task MarkAsDoneAsync_ShouldThrow_WhenNotFound()
    {
        var context = CreateDbContext();
        var userId = Guid.NewGuid().ToString();
        var currentUserService = CreateUserService(userId);
        var service = new ToDoItemsService(context, currentUserService.Object);

        await Assert.ThrowsAsync<ItemNotFoundException>(() =>
            service.MarkAsDoneAsync(999));
    }

    [Fact]
    public async Task DeleteToDoItemAsync_ShouldDelete_WhenOwned()
    {
        var context = CreateDbContext();
        var userId = Guid.NewGuid().ToString();
        var currentUserService = CreateUserService(userId);
        var service = new ToDoItemsService(context, currentUserService.Object);

        context.Users.Add(new User { Id = userId, Name = "Test User" });
        var item = new ToDoItem
        {
            Description = "Test",
            UserId = userId
        };
        context.ToDoItems.Add(item);
        await context.SaveChangesAsync();

        await service.DeleteToDoItemAsync(item.Id);

        var deleted = await context.ToDoItems.FindAsync(item.Id);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task DeleteToDoItemAsync_ShouldThrow_WhenNotOwned()
    {
        var context = CreateDbContext();
        var userId = Guid.NewGuid().ToString();
        var otherUserId = Guid.NewGuid().ToString();
        var currentUserService = CreateUserService(userId);
        var service = new ToDoItemsService(context, currentUserService.Object);

        context.Users.Add(new User { Id = otherUserId, Name = "Other" });
        var item = new ToDoItem
        {
            Description = "Test",
            UserId = otherUserId
        };
        context.ToDoItems.Add(item);
        await context.SaveChangesAsync();

        await Assert.ThrowsAsync<UserDoesNotOwnItemException>(() =>
            service.DeleteToDoItemAsync(item.Id));
    }

    [Fact]
    public async Task DeleteToDoItemAsync_ShouldThrow_WhenNotFound()
    {
        var context = CreateDbContext();
        var userId = Guid.NewGuid().ToString();
        var currentUserService = CreateUserService(userId);
        var service = new ToDoItemsService(context, currentUserService.Object);

        await Assert.ThrowsAsync<ItemNotFoundException>(() =>
            service.DeleteToDoItemAsync(999));
    }

    [Fact]
    public async Task GetToDoItemsAsync_ShouldReturnOnlyOwnedItems()
    {
        var context = CreateDbContext();
        var userId = Guid.NewGuid().ToString();
        var otherUserId = Guid.NewGuid().ToString();
        var currentUserService = CreateUserService(userId);
        var service = new ToDoItemsService(context, currentUserService.Object);

        context.Users.Add(new User { Id = userId, Name = "Test User" });
        context.Users.Add(new User { Id = otherUserId, Name = "Other" });

        context.ToDoItems.Add(new ToDoItem
        {
            Description = "Mine",
            UserId = userId,
            IsDone = false,
            DueDate = DateTime.UtcNow.AddDays(1)
        });
        context.ToDoItems.Add(new ToDoItem
        {
            Description = "Not Mine",
            UserId = otherUserId,
            IsDone = true,
            DueDate = DateTime.UtcNow.AddDays(2)
        });
        await context.SaveChangesAsync();

        var items = await service.GetToDoItemsAsync();

        Assert.Single(items);
        Assert.Equal("Mine", items[0].Description);
        Assert.False(items[0].IsDone);
    }
}
