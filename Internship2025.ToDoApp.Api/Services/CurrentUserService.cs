using System.Security.Claims;
using Internship2025.ToDoApp.Domain.Services;

namespace Internship2025.ToDoApp.Api.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public string GetUserId()
    {
        var context = httpContextAccessor.HttpContext;
        return context?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
