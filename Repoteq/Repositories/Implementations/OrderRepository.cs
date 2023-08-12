using Microsoft.EntityFrameworkCore;
using Repoteq.Data;
using Repoteq.Models;
using Repoteq.Repositories.interfaces;

namespace Repoteq.Repositories.Implementations
{
    public class OrderRepository : IRepoteqRepository<Order>
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order> GetById(int id)
        {
            return await _context.Orders.SingleOrDefaultAsync(x => x.OrderId == id);
        }

        public async Task<Order> Add(Order order)
        {
            await _context.Orders.AddAsync(order);
            _context.SaveChanges();
            return order;
        }

        public Order Update(Order order)
        {
            _context.Update(order);
            _context.SaveChanges();
            return order;
        }

        public Order Delete(Order order)
        {
            _context.Remove(order);
            _context.SaveChanges();
            return order;
        }

        public IEnumerable<Order> Search(string term)
        {
            var result = _context.Orders.Where(b => b.CustomerName.Contains(term));
                        

            return result;
        }

        public IQueryable<Order> GetNoTranckingTable()
        {
            return _context.Orders.AsNoTracking();
        }
    }
}
