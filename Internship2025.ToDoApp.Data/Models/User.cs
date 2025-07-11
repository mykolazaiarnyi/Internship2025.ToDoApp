namespace Internship2025.ToDoApp.Data.Models;

public class User
{
    public string Id { get; set; }

    public string Name { get; set; }

    public ICollection<ToDoItem> ToDoItems { get; set; } = [];
}
