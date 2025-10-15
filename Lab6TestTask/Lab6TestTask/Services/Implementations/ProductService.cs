using Lab6TestTask.Data;
using Lab6TestTask.Models;
using Lab6TestTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Lab6TestTask.Enums;

namespace Lab6TestTask.Services.Implementations;

/// <summary>
/// ProductService.
/// Implement methods here.
/// </summary>
public class ProductService : IProductService
{
    private readonly ApplicationDbContext _dbContext;

    public ProductService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product> GetProductAsync()
        => (await _dbContext.Products
            .Where(x => x.Status == ProductStatus.Reserved)
            .OrderByDescending(p => p.Price)
            .AsNoTracking()
            .FirstOrDefaultAsync())!;

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        return await _dbContext.Products
            .Where(x => x.ReceivedDate.Year == 2025 && x.Quantity > 1000)
            .AsNoTracking()
            .ToListAsync();
    }
}
