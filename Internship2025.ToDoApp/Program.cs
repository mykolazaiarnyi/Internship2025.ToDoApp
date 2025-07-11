using Internship2025.ToDoApp.Data;
using Internship2025.ToDoApp.Data.Models;

var context = new ToDoAppDbContext();

var item = context.ToDoItems.FirstOrDefault();

// mark todo items as done
item.IsDone = true;
context.SaveChanges();
