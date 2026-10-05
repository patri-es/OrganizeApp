using OrganizeApp.Application.Category.DTOs;
using OrganizeApp.Application.Category.Interfaces;

namespace OrganizeApp.Application.Category;


public class CategoryUseCases(ICategoryRepository categoryRepository)
{
    // Constructor
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    // Queries
    public async Task<List<CategoryDTO>> GetCategories()
    {
        var categories = await _categoryRepository.GetCategoriesAsync();

        return [.. categories
            .Select(category => new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Code = category.Code
            })];
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

    // Creation
    public async Task CreateCategory(CreateCategoryDTO dto)
    {
        var category = new Domain.Entities.Category(
            dto.Name,
            dto.Code,
            dto.Description,
            dto.ImageUrl);

        await _categoryRepository.AddAsync(category);
        await _categoryRepository.SaveChangesAsync();
    }

    // Deletion
    public async Task DeleteCategory(int categoryId)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId) ?? throw new Exception("Category not found");
        await _categoryRepository.DeleteAsync(category);
        await _categoryRepository.SaveChangesAsync();
    }

    // Update 
    public async Task UpdateCategory(int categoryId, UpdateCategoryDTO dto)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId) ?? throw new Exception("Category not found");

        category.ChangeName(dto.Name);
        category.ChangeDescription(dto.Description);
        category.ChangeImage(dto.ImageUrl);
    }
    // Edition
    public async Task ChangeCategoryName(int categoryId, string name)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId) ?? throw new Exception("Category not found");
        category.ChangeName(name);

        await _categoryRepository.SaveChangesAsync();
    }

    public async Task ChangeCategoryDescription(int categoryId, string description)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId) ?? throw new Exception("Category not found");
        category.ChangeDescription(description);

        await _categoryRepository.SaveChangesAsync();
    }

    public async Task ChangeCategoryCode(int categoryId, string code)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId) ?? throw new Exception("Category not found");
        category.ChangeCode(code);

        await _categoryRepository.SaveChangesAsync();
    }

    public async Task ChangeCategoryImage(int categoryId, string imageUrl)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId) ?? throw new Exception("Category not found");
        category.ChangeImage(imageUrl);

        await _categoryRepository.SaveChangesAsync();
    }


}
