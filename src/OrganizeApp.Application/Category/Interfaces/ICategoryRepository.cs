
using OrganizeApp.Application.Category.DTOs;
using OrganizeApp.Application.DTOs;
using OrganizeApp.Domain.Entities;

public interface ICategoryRepository
{
    Task<List<Category>> GetCategorysAsync(CategoryDTO? filters);

    Task<Category?> GetByIdAsync(int categoryId);

    Task AddAsync(Category category);

    Task DeleteAsync(Category category);

    Task SaveChangesAsync();
    Task AddAsync(NewCategoryDTO category);
}