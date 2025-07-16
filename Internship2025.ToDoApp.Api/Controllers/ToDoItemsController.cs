using Internship2025.ToDoApp.Domain.DTOs;
using Internship2025.ToDoApp.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Internship2025.ToDoApp.Api.Controllers;

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
    public IActionResult AddToDoItem(CreateToDoItemDto itemDto)
    {
        _toDoItemsService.AddToDoItem(itemDto);

        return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult EditToDoItem(int id, UpdateToDoItemDto itemDto)
    {
        _toDoItemsService.EditToDoItem(id, itemDto);
        return Ok();
    }

    [HttpPut("{id}/status")]
    public IActionResult MarkAsDone(int id)
    {
        _toDoItemsService.MarkAsDone(id);
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteToDoItem(int id)
    {
        _toDoItemsService.DeleteToDoItem(id);
        return Ok();
    }

    [HttpGet]
    public ActionResult<List<GetToDoItemsListDto>> GetToDoItems()
    {
        var items = _toDoItemsService.GetToDoItems();
        return Ok(items);
    }
}
