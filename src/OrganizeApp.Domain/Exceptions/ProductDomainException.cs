namespace OrganizeApp.Domain.Exceptions;

public sealed class ProductDomainException : Exception
{
    public ProductDomainException(string message)
        : base(message)
    {
    }

    public ProductDomainException(
        string message,
        Exception innerException)
        : base(message, innerException)
    {
    }
}