namespace OrganizeApp.Domain.Exceptions;

public sealed class CategoryDomainException : Exception
{
    public CategoryDomainException(string message)
        : base(message)
    {
    }

    public CategoryDomainException(
        string message,
        Exception innerException)
        : base(message, innerException)
    {
    }
}