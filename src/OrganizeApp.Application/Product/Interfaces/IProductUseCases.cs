
using OrganizeApp.Application.Product.DTOs;

namespace OrganizeApp.Application.Product.Interfaces;

public interface IProductUseCases 
{
    Task<List<ProductDTO>> GetProducts(FiltersProductDTO? filters);

    Task<ProductDTO?> GetProductById(int productId);

    Task CreateProduct(CreateProductDTO product);

    Task DeleteProduct(int productId);

    Task UpdateProduct(int productId, UpdateProductDTO product);

    Task IncreaseProductStock(int productId, int quantity);

    Task DecreaseProductStock(int productId, int quantity);

    Task ActivateProduct(int productId);

    Task DeactivateProduct(int productId);

    Task AssignProductCategory(int productId, int categoryId);

    Task RemoveProductCategory(int productId, int categoryId);
}