using System.Net;

namespace Internship2025.ToDoApp.Domain.Exceptions;

public class UserDoesNotOwnItemException : DomainException
{
    public UserDoesNotOwnItemException() : base("User does not own this item.", (int)HttpStatusCode.Forbidden)
    {
    }
}
