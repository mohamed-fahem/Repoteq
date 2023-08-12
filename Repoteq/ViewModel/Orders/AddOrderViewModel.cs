using Microsoft.AspNetCore.Mvc.Rendering;
using Repoteq.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repoteq.ViewModel.Orders
{
    public class AddOrderViewModel
    {

        public string CustomerName { get; set; }
        public int OrderNumber { get; set; }



        public IEnumerable<Product> ListItemss { get; set; }

        public IEnumerable<SelectListItem> ListProducts { get; set; }



    }
}
