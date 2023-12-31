﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repoteq.Data;
using Repoteq.Models;
using Repoteq.Repositories.interfaces;
using Repoteq.ViewModel.Orders;
using X.PagedList;

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
        public async Task<IActionResult> Index(int? page)
        {
            var orders = await _orderRepository.GetAll();
            int pageNumber = page ?? 1;
            int pageSize = 5;

            var pagedOrders = orders.ToPagedList(pageNumber, pageSize);

            return View(pagedOrders);
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


        public async Task<IActionResult> Search(OrderSearchViewModel searchModel, int? page)
        {
            var orders = await _orderRepository.GetAll();

            // Apply filters based on the search model properties

            if (searchModel.OrderNumber.HasValue)
            {
                orders = orders.Where(o => o.OrderCode == searchModel.OrderNumber.Value);
            }

            if (!string.IsNullOrEmpty(searchModel.CustomerName))
            {
                orders = orders.Where(o => o.CustomerName.Contains(searchModel.CustomerName));
            }

            if (searchModel.Total.HasValue)
            {
                orders = orders.Where(o => o.Total == searchModel.Total.Value);
            }

            if (searchModel.FromDate.HasValue)
            {
                orders = orders.Where(o => o.Date >= searchModel.FromDate.Value);
            }

            if (searchModel.ToDate.HasValue)
            {
                orders = orders.Where(o => o.Date <= searchModel.ToDate.Value);
            }

            int pageNumber = page ?? 1;
            int pageSize = 5;

            var pagedOrders = orders.ToPagedList(pageNumber, pageSize);

            return View("Index", pagedOrders);
        }
    }
}
