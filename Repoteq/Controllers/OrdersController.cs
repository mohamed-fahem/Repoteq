using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repoteq.Data;
using Repoteq.Models;
using Repoteq.Repositories.interfaces;
using Repoteq.ViewModel.Orders;

namespace Repoteq.Controllers
{

    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IRepoteqRepository<Order> _orderRepository;
        private readonly IRepoteqRepository<Product> _productRepository;
        private readonly ApplicationDbContext _context;

        public OrdersController(IRepoteqRepository<Order> orderRepository,
            IRepoteqRepository<Product> productRepository, ApplicationDbContext context)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _context = context;
        }


        // GET: OrdersController
        public async Task<IActionResult> Index()
        {
            var orders = await _orderRepository.GetAll();
            return View(orders);
        }

        // GET: OrdersController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderRepository.GetById(id);
            return View(order);
        }


        public async Task<IActionResult> Create()
        {
            var model = new AddOrderViewModel();

            model.ListProducts = _context.Products.Select(s => new SelectListItem
            {
                Text = s.ProductName,
                Value = s.ProductId.ToString()
            }).ToList();

            return View(model);


        }



        public IActionResult GetProductPrice(int? productId)
        {
            if (productId != null)
                return Json(_context.Products.Find(productId).Price);
            return Json("0");
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrder(AddOrderDTO model)
        {

            var order = new Order
            {
                CustomerName = model.CustomerName,
                Date = DateTime.Now,
                OrderCode = int.Parse(model.OrderNumber),
                Total = model.Items.Select(s => s.Quantity * s.Price).Sum()
            };
            _context.Orders.Add(order);
            _context.SaveChanges();


            foreach (var item in model.Items)
            {
                _context.OrderItems.Add(new OrderItem
                {
                    OrderId = order.OrderId,
                    Price = (decimal)item.Price,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                });
            }

            _context.SaveChanges();

            return Json(new { code = "1" });
        }

        // GET: OrdersController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(a => a.Items)
                .ThenInclude(a => a.Product)
                .FirstOrDefaultAsync(a => a.OrderId == id);


            if (order == null) return NotFound();

            var model = new EditOrderViewModel
            {
                OrderId = order.OrderId,
                CustomerName = order.CustomerName,
                OrderNumber = order.OrderCode,
                FinalTotal=(decimal)order.Total,
                ListProducts = _context.Products.Select(s => new SelectListItem
                {
                    Text = s.ProductName,
                    Value = s.ProductId.ToString()
                }).ToList(),
                OrderItemsList = order.Items.ToList()
            };

            return View(model);

        }

        // POST: OrdersController/Edit/5
        [HttpPost]

        public async Task<IActionResult> SaveEdit(EditOrderDTO model)

        {
            var order = new Order
            {
                OrderId = model.OrderId,
                CustomerName = model.CustomerName,
                Date = DateTime.Now,
                OrderCode = int.Parse(model.OrderNumber),
                Total = model.Items.Select(s => s.Quantity * s.Price).Sum(),

            };
            _context.Orders.Update(order);
            _context.SaveChanges();



            // delete old items
            _context.RemoveRange(_context.OrderItems.Where(x => x.OrderId == model.OrderId).ToList());

            // add new items
            foreach (var item in model.Items)
            {
                _context.OrderItems.Add(new OrderItem
                {
                    OrderId = order.OrderId,
                    Price = (decimal)item.Price,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                });
            }
            _context.SaveChanges();

            return Json(new { code = "1" });
        }



        // GET: OrdersController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderRepository.GetById(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: OrdersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Order order)
        {
            try
            {
                _orderRepository.Delete(order);
                return RedirectToAction(nameof(Index));
            }

            catch
            {
                return View();
            }
        }


        public IActionResult Search(string term)
        {
            var result = _orderRepository.Search(term);

            return View("Index", result);
        }
    }
}
