namespace OrganizeApp.Domain.Exceptions;

public class ProductsDomainExceptions : Exception
{
    public ProductsDomainExceptions()
    { }

    public ProductsDomainExceptions(string message)
        : base(message)
    { }

    public ProductsDomainExceptions(string message, Exception innerException)
        : base(message, innerException)
    { }
}
