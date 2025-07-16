namespace Internship2025.ToDoApp.Domain.DTOs;

public class GetToDoItemsListDto
{
    public int Id { get; set; }
    
    public string Description { get; set; }
    
    public bool IsDone { get; set; }
    
    public DateTime? DueDate { get; set; }
}
