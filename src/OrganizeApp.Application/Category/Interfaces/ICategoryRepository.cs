using OrganizeApp.Application.Category.DTOs;
using OrganizeApp.Application.DTOs;
using CategoryEntity = OrganizeApp.Domain.Entities.Category;

namespace OrganizeApp.Application.Category.Interfaces;
public interface ICategoryRepository
{
    Task<List<CategoryEntity>> GetCategoriesAsync();

    Task<CategoryEntity?> GetByIdAsync(int categoryId);

    Task DeleteAsync(CategoryEntity category);

    Task SaveChangesAsync();
    Task AddAsync(CreateCategoryDTO category);
}