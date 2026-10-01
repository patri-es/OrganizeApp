using OrganizeApp.Application.Category.DTOs;
using OrganizeApp.Application.DTOs;

namespace OrganizeApp.Application.Category;


public class CategoryUseCases
{
    // Constructor
    private readonly ICategoryRepository _categoryRepository;

    public CategoryUseCases(ICategoryRepository categoryRepository)
    {
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

    // Creation
    public async Task CreateCategory(CreateCategoryDTO dto)
    {
        var category = new Domain.Entities.Category(
            dto.Name,
            dto.Code,
            dto.Description,
            dto.ImageUrl);

        await _categoryRepository.AddAsync(dto);
        await _categoryRepository.SaveChangesAsync();
        //throw new NotImplementedException();
    }

    // Deletion
    public async Task DeleteCategory(int categoryId)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);

        if (category is null)
            throw new Exception("Category not found");

        await _categoryRepository.DeleteAsync(category);
        await _categoryRepository.SaveChangesAsync();
    }

    // Edition
    public async Task ChangeCategoryName(int categoryId, string name)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);

        if (category is null)
            throw new Exception("Category not found");

        category.ChangeName(name);

        await _categoryRepository.SaveChangesAsync();
    }

    public async Task ChangeCategoryDescription(int categoryId, string description)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);

        if (category is null)
            throw new Exception("Category not found");

        category.ChangeDescription(description);

        await _categoryRepository.SaveChangesAsync();
    }

    public async Task ChangeCategoryCode(int categoryId, string code)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);

        if (category is null)
            throw new Exception("Category not found");

        category.ChangeCode(code);

        await _categoryRepository.SaveChangesAsync();
    }

    public async Task ChangeCategoryImage(int categoryId, string imageUrl)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);

        if (category is null)
            throw new Exception("Category not found");

        category.ChangeImage(imageUrl);

        await _categoryRepository.SaveChangesAsync();
    }


}
