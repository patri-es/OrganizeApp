namespace OrganizeApp.Domain.Exceptions;

public class CategoryDomainExceptions : Exception
{
    public CategoryDomainExceptions()
    { }

    public CategoryDomainExceptions(string message)
        : base(message)
    { }

    public CategoryDomainExceptions(string message, Exception innerException)
        : base(message, innerException)
    { }
}
