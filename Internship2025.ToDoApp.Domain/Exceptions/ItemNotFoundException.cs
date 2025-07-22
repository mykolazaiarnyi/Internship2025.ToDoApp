using System.Net;

namespace Internship2025.ToDoApp.Domain.Exceptions;

public class ItemNotFoundException : DomainException
{
    public ItemNotFoundException() : base("Item not found.", (int)HttpStatusCode.NotFound)
    {
    }
}
