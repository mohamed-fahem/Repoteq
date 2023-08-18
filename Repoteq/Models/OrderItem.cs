using System.ComponentModel.DataAnnotations;

namespace Repoteq.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }


        public decimal Price { get; set; }
        public decimal? PriceAfterDiscount { get; set; }

        public decimal Total
        {
            get { return (Quantity * (PriceAfterDiscount ?? Price)); }
        }


        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
    }
}
