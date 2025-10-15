using Lab6TestTask.Data;
using Lab6TestTask.Models;
using Lab6TestTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Lab6TestTask.Enums;

namespace Lab6TestTask.Services.Implementations;

/// <summary>
/// WarehouseService.
/// Implement methods here.
/// </summary>
public class WarehouseService : IWarehouseService
{
    private readonly ApplicationDbContext _dbContext;

    public WarehouseService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Warehouse> GetWarehouseAsync()
    {
        return (await _dbContext.Warehouses
            .Select(x => new
            {
                Warehouse = x,
                TotalPrice = x.Products
                    .Where(p => p.Status == ProductStatus.ReadyForDistribution)
                    .Sum(p => p.Price * p.Quantity)
            })
            .OrderByDescending(x => x.TotalPrice)
            .Select(x => x.Warehouse)
            .AsNoTracking()
            .FirstOrDefaultAsync())!;
    }

    public async Task<IEnumerable<Warehouse>> GetWarehousesAsync()
    {
        var startDate = new DateTime(2025, 4, 1);
        var endDate = new DateTime(2025, 7, 1);
        return await _dbContext.Warehouses
             .Where(x => x.Products.Any(y => y.ReceivedDate >= startDate && y.ReceivedDate < endDate))
             .AsNoTracking()
             .ToListAsync();
    }
}
