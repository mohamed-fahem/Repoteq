using Repoteq.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repoteq.ViewModel.Orders
{
    public class AddOrderViewModel
    {
        
        public string CustomerName { get; set; }
        public int OrderNumber { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        [Display(Name = "Product")]
        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
