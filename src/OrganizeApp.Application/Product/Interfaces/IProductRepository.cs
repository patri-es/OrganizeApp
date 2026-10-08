
using OrganizeApp.Application.Product.DTOs;
using ProductEntity = OrganizeApp.Domain.Entities.Product;

namespace OrganizeApp.Application.Product.Interfaces;

public interface IProductRepository
{
    Task<List<ProductEntity>> GetProductsAsync(FiltersProductDTO? filters);

    Task<ProductEntity?> GetByIdAsync(int productId);

    Task AddAsync(ProductEntity product);

    Task DeleteAsync(ProductEntity product);

    Task SaveChangesAsync();
}