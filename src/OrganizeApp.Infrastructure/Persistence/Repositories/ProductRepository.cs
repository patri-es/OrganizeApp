using Microsoft.EntityFrameworkCore;
using OrganizeApp.Application.Product.DTOs;
using OrganizeApp.Application.Product.Interfaces;
using OrganizeApp.Infrastructure.Persistence;
using ProductEntity = OrganizeApp.Domain.Entities.Product;

namespace OrganizeApp.Infrastructure.Persistence.Repositories;

public class ProductRepository(AppDbContext context) : IProductRepository
{
    private readonly AppDbContext _context = context;

    // Get Products with filters
    public async Task<List<ProductEntity>> GetProductsAsync(
    FiltersProductDTO? filters)
    {
        IQueryable<ProductEntity> query = _context.Products;

        if (filters is not null)
        {
            if (filters.CategoryId.HasValue)
            {
                query = query.Where(p =>
                    p.Categories.Any(c => c.Id == filters.CategoryId.Value));
            }

            if (filters.HasCategory.HasValue)
            {
                query = filters.HasCategory.Value
                    ? query.Where(p => p.Categories.Any())
                    : query.Where(p => !p.Categories.Any());
            }

            if (filters.IsAvailable.HasValue)
            {
                query = query.Where(p =>
                    p.IsAvailable == filters.IsAvailable.Value);
            }

            if (filters.MinStock.HasValue)
            {
                query = query.Where(p =>
                    p.Stock >= filters.MinStock.Value);
            }

            if (filters.MaxStock.HasValue)
            {
                query = query.Where(p =>
                    p.Stock <= filters.MaxStock.Value);
            }
        }

        return await query.ToListAsync();
    }

    public async Task<ProductEntity?> GetByIdAsync(int productId)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Id == productId);
    }

    public async Task AddAsync(ProductEntity product)
    {
        await _context.Products.AddAsync(product);
    }

    public async Task DeleteAsync(ProductEntity product)
    {
        _context.Products.Remove(product);

        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}