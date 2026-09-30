using OrganizeApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizeApp.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<Product> GetAsync(int id);
    Task<Product> GetByCodeAsync(string code);
    Task AddAsync(Product product);
    Task<bool> ExistAsync(int id);
    Task<Product> DeleteAsync(int id);
}
