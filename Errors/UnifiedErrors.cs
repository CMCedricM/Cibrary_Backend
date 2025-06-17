using System;

namespace Cibrary_Backend.Errors;

public class ForbiddenFieldException(string message, List<string> fields) : Exception(message)
{

    public List<string> ForbiddenFields { get; } = fields;
    public int StatusCode { get; } = 400;
}


public class DataNotFound(string message, string isbn, string title) : Exception(message)
{
    public string Title { get; } = title;
    public string Isbn { get; } = isbn;

    public int StatusCode { get; } = 404;
}