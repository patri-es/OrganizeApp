
using OrganizeApp.Application.Product.DTOs;
using OrganizeApp.Domain.Entities;


public interface IProductRepository
{

    Task<Product?> GetByIdAsync(int productId);

    Task AddAsync(Product product);

    Task DeleteAsync(Product product);

    Task SaveChangesAsync();
    Task AddAsync(CreateProductDTO product);
}