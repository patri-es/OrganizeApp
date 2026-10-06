
using OrganizeApp.Application.Category.DTOs;
using CategoryEntity = OrganizeApp.Domain.Entities.Category;
using OrganizeApp.Application.Category;

namespace OrganizeApp.Application.Category.Interfaces;

public interface ICategoryUseCases 
{
    Task<List<CategoryDTO>> GetCategories();

    Task<CategoryDTO?> GetCategoryById(int categoryId);

    Task CreateCategory(CreateCategoryDTO category);

    Task DeleteCategory(int categoryId);

    Task UpdateCategory(int categoryId, UpdateCategoryDTO category);

    Task ChangeCategoryName(int categoryId, string name);

    Task ChangeCategoryDescription(int categoryId, string description);

    Task ChangeCategoryCode(int categoryId, string code);

    Task ChangeCategoryImage(int categoryId, string imageUrl);

    //Task SaveChangesAsync();
}