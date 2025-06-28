using System;
using Auth0.ManagementApi.Models;

namespace Cibrary_Backend.Errors;


public abstract class GeneralException(string message) : Exception(message)
{
    public abstract int StatusCode { get; }
    public virtual object ToErrorResponse() => new
    {
        StatusCode,
        Message = Message,
    };
}
public class ForbiddenFieldException(string message, List<string> fields) : GeneralException(message)
{

    public List<string> ForbiddenFields { get; } = fields;
    public override int StatusCode => 400;

    public override object ToErrorResponse() => new
    {
        StatusCode,
        Message,
        ForbiddenFields
    };

}


public class DataNotFound(string message, string isbn, string title) : GeneralException(message)
{
    public string Title { get; } = title;
    public string Isbn { get; } = isbn;

    public override int StatusCode => 404;

    public override object ToErrorResponse() => new
    {
        StatusCode,
        Message,
        Isbn,
        Title
    };

}


public class ConflictFound(string message, string isbn, string title) : GeneralException(message)
{
    public override int StatusCode => 409;
    public string Title { get; } = title;
    public string Isbn { get; } = isbn;

    public override object ToErrorResponse() => new
    {
        StatusCode,
        Message,
        Isbn,
        Title
    };
}