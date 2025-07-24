using Internship2025.ToDoApp.Domain.DTOs;
using Internship2025.ToDoApp.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Internship2025.ToDoApp.Api.Controllers;

[Authorize]
[Route("api/to-do-items")]
[ApiController]
public class ToDoItemsController : ControllerBase
{
    private readonly ToDoItemsService _toDoItemsService;

    public ToDoItemsController(ToDoItemsService toDoItemsService)
    {
        _toDoItemsService = toDoItemsService;
    }

    [HttpPost]
    public async Task<IActionResult> AddToDoItem(CreateToDoItemDto itemDto)
    {
        await _toDoItemsService.AddToDoItemAsync(itemDto);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditToDoItem(int id, UpdateToDoItemDto itemDto)
    {
        await _toDoItemsService.UpdateToDoItemAsync(id, itemDto);
        return Ok();
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> MarkAsDone(int id)
    {
        await _toDoItemsService.MarkAsDoneAsync(id);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteToDoItem(int id)
    {
        await _toDoItemsService.DeleteToDoItemAsync(id);
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<List<GetToDoItemsListDto>>> GetToDoItems()
    {
        var items = await _toDoItemsService.GetToDoItemsAsync();
        return Ok(items);
    }
}
