using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repoteq.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Repoteq.Repositories.interfaces;
using Microsoft.AspNetCore.Authorization;
using Repoteq.ViewModel.Products;

namespace Repoteq.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IRepoteqRepository<Product> _productRepository;
        private readonly IHostingEnvironment _hosting;


        public ProductsController(IRepoteqRepository<Product> productRepository, IHostingEnvironment hosting)
        {
            _productRepository = productRepository;
            _hosting = hosting;
        }


        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAll();
            return View(products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productRepository.GetById(id);
            return View(product);
        }


        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var model = new ProductViewModel();
            return View(model);
        }

        // POST: Products/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            try
            {
                string filename = UploadFile(model.ProductImage) ?? string.Empty;
                var product = new Product
                {
                    ProductId = model.ProductId,
                    Date = DateTime.Now,
                    ProductName = model.ProductName,
                    Price = model.Price,
                    Image=filename
                };
                await _productRepository.Add(product);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            if(id==null) return NotFound();
            
            var product = await _productRepository.GetById(id);
            if(product==null) return NotFound();

            var model = new ProductViewModel
            {
                ProductId = product.ProductId,
                Price = product.Price,
                ProductName = product.ProductName,
                Image=product.Image
                
            };


            return View(model);
        }

        // POST: Products/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductViewModel model)
        {
            try
            {
                string filename = UploadFile(model.ProductImage, model.Image);

                var product = new Product
                {
                    ProductId = model.ProductId,
                    Date = DateTime.Now,
                    ProductName = model.ProductName,
                    Price = model.Price,
                    Image = filename
                };
                _productRepository.Update(product);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Product product)
        {
            try
            {
                _productRepository.Delete(product);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }



        //public IActionResult Search(string term)
        //{
        //    var result = _productRepository.Search(term);

        //    return View("Index", result);
        //}





        string UploadFile(IFormFile file)
        {
            if (file != null)
            {
                string uploads = Path.Combine(_hosting.WebRootPath, "uploads");
                string fullpath = Path.Combine(uploads, file.FileName);
                file.CopyTo(new FileStream(fullpath, FileMode.Create));
                return file.FileName;
            }

            return null;
        }

        string UploadFile(IFormFile file, string imageUrl)
        {
            if (file != null)
            {
                string upload = Path.Combine(_hosting.WebRootPath, "uploads");
                string newpath = Path.Combine(upload, file.FileName);

                // Delete the old file
                string oldpath = Path.Combine(upload, imageUrl);
                if (oldpath != newpath)
                {
                    System.IO.File.Delete(oldpath);

                    // Save new file
                    file.CopyTo(new FileStream(newpath, FileMode.Create));
                }
                return file.FileName;

            }

            return imageUrl;
        }
    }
}
