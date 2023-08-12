using Repoteq.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Repoteq.ViewModel.Orders
{
    public class EditOrderViewModel
    {
        public int Id { get; set; }
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
