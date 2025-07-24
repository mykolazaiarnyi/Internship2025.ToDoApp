using Microsoft.AspNetCore.Identity;

namespace Internship2025.ToDoApp.Data.Models;

public class User : IdentityUser
{
    public ICollection<ToDoItem> ToDoItems { get; set; } = [];
}
