using Microsoft.EntityFrameworkCore;
using Repoteq.Data;
using Repoteq.Models;
using Repoteq.Repositories.interfaces;

namespace Repoteq.Repositories.Implementations
{
    public class ProductRepository : IRepoteqRepository<Product>
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> Add(Product product)
        {
            await _context.AddAsync(product);
            _context.SaveChanges();
            return product;
        }

        public Product Delete(Product product)
        {
            _context.Remove(product);
            _context.SaveChanges();
            return product;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _context.Products.Include(x=>x.Orders).ToListAsync();
        }

        public async Task<Product> GetById(int id)
        {
            return await _context.Products.SingleOrDefaultAsync(x => x.ProductId == id);

        }

        public IQueryable<Product> GetNoTranckingTable()
        {
            return _context.Products.AsNoTracking();
        }

        

        public Product Update(Product product)
        {
            _context.Update(product);
            _context.SaveChanges();
            return product;
        }
    }
}
