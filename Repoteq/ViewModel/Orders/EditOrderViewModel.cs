using Repoteq.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Repoteq.ViewModel.Orders
{
    public class EditOrderViewModel
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public int OrderNumber { get; set; }



        

        public IEnumerable<SelectListItem> ListProducts { get; set; }

        public IEnumerable<OrderItem> OrderItemsList { get; set; }
    }
}
