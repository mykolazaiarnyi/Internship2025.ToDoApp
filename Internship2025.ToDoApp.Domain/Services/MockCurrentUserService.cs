using Internship2025.ToDoApp.Data;

namespace Internship2025.ToDoApp.Domain.Services;

public class MockCurrentUserService : ICurrentUserService
{
    public string GetUserId()
    {
        return ToDoAppDbContext.SeededUserId;
    }
}
