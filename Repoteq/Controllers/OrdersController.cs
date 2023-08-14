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

        public OrdersController(IRepoteqRepository<Order> orderRepository, IRepoteqRepository<Product> productRepository, ApplicationDbContext context)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _context = context;
        }


        public IActionResult AddMorePartialView()
        {

            OrderItem model = new OrderItem();
            return PartialView("View", model);
        }


        public IActionResult orderItemTable(int? productId)
        {
            
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order model)
        {
            try
            {
                var product = await _productRepository.GetById(model.OrderId);

                foreach (var item in model.Items)
                {
                    var orderItem = new OrderItem
                    {
                        Product = product,
                        Price = product.Price,
                        Quantity = item.Quantity,
                        PriceAfterDiscount = item.PriceAfterDiscount,

                    };
                    _context.OrderItems.Add(orderItem);
                    _context.SaveChanges();

                    var order = new Order
                    {
                        CustomerName = model.CustomerName,
                        Date = DateTime.Now,
                        OrderCode = model.OrderCode,
                        Total = (int)orderItem.Total
                    };
                    await _orderRepository.Add(order);

                }

                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return View();
            }
        }

        // GET: OrdersController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null) return NotFound();

            var getorder = await _orderRepository.GetById(id);
            if (getorder == null) return NotFound();




            var order = new Order
            {
                CustomerName = getorder.CustomerName,
                OrderCode = getorder.OrderCode,
                Items = (ICollection<OrderItem>)getorder.Items.Select(item => new OrderItem
                {
                    Price = item.Price,
                    Quantity = item.Quantity,
                    Product = item.Product,
                    PriceAfterDiscount = item.PriceAfterDiscount
                })
            };



            return View(order);
        }

        // POST: OrdersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Order model)
        {
            try
            {
                var product = await _productRepository.GetById(model.OrderId);

                foreach (var item in model.Items)
                {
                    var orderItem = new OrderItem
                    {
                        Product = product,
                        Price = product.Price,
                        Quantity = item.Quantity,
                        PriceAfterDiscount = item.PriceAfterDiscount,

                    };
                    _context.OrderItems.Add(orderItem);
                    _context.SaveChanges();

                    var order = new Order
                    {
                        CustomerName = model.CustomerName,
                        Date = DateTime.Now,
                        OrderCode = model.OrderCode,
                        Total = (int)orderItem.Total
                    };
                    _orderRepository.Update(order);

                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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
