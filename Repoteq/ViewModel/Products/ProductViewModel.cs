using Repoteq.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repoteq.ViewModel.Products
{
    public class ProductViewModel
    {
        [Key]
        public int ProductId { get; set; }
        [NotMapped]
        public IFormFile ProductImage { get; set; }
        public string Image { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }
        public Product Product { get; set; }
    }
}
