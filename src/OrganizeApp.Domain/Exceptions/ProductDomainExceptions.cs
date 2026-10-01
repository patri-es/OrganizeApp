namespace OrganizeApp.Domain.Exceptions;

public class ProductDomainExceptions : Exception
{
    public ProductDomainExceptions()
    { }

    public ProductDomainExceptions(string message)
        : base(message)
    { }

    public ProductDomainExceptions(string message, Exception innerException)
        : base(message, innerException)
    { }
}
