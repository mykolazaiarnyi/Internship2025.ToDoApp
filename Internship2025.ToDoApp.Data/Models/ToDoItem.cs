namespace Internship2025.ToDoApp.Data.Models;

public class ToDoItem
{
    public int Id { get; set; }

    public string Description { get; set; }

    public bool IsDone { get; set; }

    public string UserId { get; set; }

    public User User { get; set; }
}
