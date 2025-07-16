using Internship2025.ToDoApp.Domain.DTOs;

namespace Internship2025.ToDoApp.Domain.Services;

public class ToDoItemsService
{
    //1. Додати завдання
    public void AddToDoItem(CreateToDoItemDto itemDto)
    {
    }

    //2. Відредагувати(опис + дата)
    public void EditToDoItem(int id, UpdateToDoItemDto itemDto)
    {

    }

    //3. Позначити як виконане
    public void MarkAsDone(int id)
    {

    }

    //4. Видалити
    public void DeleteToDoItem(int id)
    {
    }

    //5. Переглянути список
    public List<GetToDoItemsListDto> GetToDoItems()
    {
        // Повертає список завдань для користувача
        return [];
    }
}
