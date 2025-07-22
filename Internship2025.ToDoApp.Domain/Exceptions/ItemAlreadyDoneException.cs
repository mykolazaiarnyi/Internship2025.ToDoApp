using System.Net;

namespace Internship2025.ToDoApp.Domain.Exceptions;

public class ItemAlreadyDoneException : DomainException
{
    public ItemAlreadyDoneException() : base("Item already marked as done.", (int)HttpStatusCode.Conflict)
    {
    }
}
