using Microsoft.EntityFrameworkCore;
using OrganizeApp.Application.Category.Interfaces;
using OrganizeApp.Infrastructure.Persistence;
using CategoryEntity = OrganizeApp.Domain.Entities.Category;

namespace OrganizeApp.Infrastructure.Persistence.Repositories;

public class CategoryRepository(AppDbContext context) : ICategoryRepository
{
    private readonly AppDbContext _context = context;

    public async Task<List<CategoryEntity>> GetCategoriesAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<CategoryEntity?> GetByIdAsync(int categoryId)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == categoryId);
    }

    public Task AddAsync(CategoryEntity category)
    {
        _context.Categories.Add(category);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(CategoryEntity category)
    {
        _context.Categories.Remove(category);

        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync(); 
    }
}