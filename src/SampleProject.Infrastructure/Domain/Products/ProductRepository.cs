namespace SampleProject.Infrastructure.Domain.Products
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Database;
    using Microsoft.EntityFrameworkCore;
    using SampleProject.Domain.Products;
    using SeedWork;

    public class ProductRepository : IProductRepository
    {
        private readonly OrdersContext _context;

        public ProductRepository(OrdersContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Product>> GetByIdsAsync(List<ProductId> ids)
        {
            return await _context
                .Products
                .IncludePaths("_prices")
                .Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context
                .Products
                .IncludePaths("_prices")
                .ToListAsync();
        }
    }
}