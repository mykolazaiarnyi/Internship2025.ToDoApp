using Internship2025.ToDoApp.Data;
using Internship2025.ToDoApp.Data.Models;
using Internship2025.ToDoApp.Domain.DTOs;
using Internship2025.ToDoApp.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Internship2025.ToDoApp.Domain.Services;

public class ToDoItemsService(ToDoAppDbContext context, ICurrentUserService currentUserService)
{
    public async Task AddToDoItemAsync(CreateToDoItemDto itemDto)
    {
        var item = new ToDoItem
        {
            Description = itemDto.Description,
            DueDate = itemDto.DueDate,
            IsDone = false,
            UserId = currentUserService.GetUserId()
        };
        await context.ToDoItems.AddAsync(item);
        await context.SaveChangesAsync();
    }

    public async Task UpdateToDoItemAsync(int id, UpdateToDoItemDto itemDto)
    {
        var item = await context.ToDoItems.FindAsync(id) 
            ?? throw new ItemNotFoundException();
        
        if (item.UserId != currentUserService.GetUserId())
        {
            throw new UserDoesNotOwnItemException();
        }

        item.Description = itemDto.Description;
        item.DueDate = itemDto.DueDate;
        await context.SaveChangesAsync();
    }

    public async Task MarkAsDoneAsync(int id)
    {
        var item = await context.ToDoItems.FindAsync(id) 
            ?? throw new ItemNotFoundException();
        
        if (item.UserId != currentUserService.GetUserId())
        {
            throw new UserDoesNotOwnItemException();
        }

        if (item.IsDone)
        {
            throw new ItemAlreadyDoneException();
        }

        item.IsDone = true;
        await context.SaveChangesAsync();
    }

    public async Task DeleteToDoItemAsync(int id)
    {
        var item = await context.ToDoItems.FindAsync(id) 
            ?? throw new ItemNotFoundException();
        
        if (item.UserId != currentUserService.GetUserId())
        {
            throw new UserDoesNotOwnItemException();
        }

        context.ToDoItems.Remove(item);
        await context.SaveChangesAsync();
    }

    public async Task<List<GetToDoItemsListDto>> GetToDoItemsAsync()
    {
        var userId = currentUserService.GetUserId();

        return await context.ToDoItems
            .Where(item => item.UserId == userId)
            .Select(item => new GetToDoItemsListDto
            {
                Id = item.Id,
                Description = item.Description,
                IsDone = item.IsDone,
                DueDate = item.DueDate
            })
            .ToListAsync();
    }
}
