using OrganizeApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizeApp.Domain.Interfaces;

public interface IProductRepository
{
    Task<Product> GetAsync(int id);
    Task AddAsync(Product product);
    Task<bool> ExistAsync(int id);
    Task<bool> IsAvailableAsync(int id);
    Task<Product> DeleteAsync(int id);
    Task<int> GetStockAsync(int id);
    Task<Product> IncreaseStockAsync(int id);
    Task<Product> DecreaseStockAsync(int id);
}
