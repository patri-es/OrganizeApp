
using OrganizeApp.Application.Product.DTOs;
using OrganizeApp.Domain.Entities;


public interface IProductRepository
{
    Task<List<Product>> GetProductsAsync(ProductFiltersDTO? filters);

    Task<Product?> GetByIdAsync(int productId);

    Task AddAsync(Product product);

    Task DeleteAsync(Product product);

    Task SaveChangesAsync();
    Task AddAsync(CreateProductDTO product);
}