using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static NuGet.Packaging.PackagingConstants;

namespace Repoteq.Models
{
    public class Order
    {
        public Order()
        {
            Items = new HashSet<OrderItem>();
        }
        public int OrderId { get; set; }
        public int OrderCode { get; set; }
        public string CustomerName { get; set; }
        public int Total { get; set; }
        public DateTime Date { get; set; }

        public virtual ICollection<OrderItem> Items { get; set; }

    }
}
