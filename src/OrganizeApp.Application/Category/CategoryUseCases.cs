using OrganizeApp.Application.Category.DTOs;
using OrganizeApp.Application.Product.DTOs;

namespace OrganizeApp.Application.Category;


public class CategoryUseCases
{
    // Constructor
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public CategoryUseCases(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    // Queries
    public async Task<List<CategoryDTO>> GetCategories()
    {
        var categories = await _categoryRepository.GetCategoriesAsync();

        return categories
            .Select(category => new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Code = category.Code
            })
            .ToList();
    }

    public async Task<CategoryDTO?> GetCategoryById(int categoryId)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);

        if (category is null)
            return null; //new Exception("Category not found");

        return new CategoryDTO
        {
            Id = category.Id,
            Name = category.Name,
            Code = category.Code
        };
    }

    // 

}
