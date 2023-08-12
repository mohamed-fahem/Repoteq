using System.ComponentModel.DataAnnotations;

namespace Repoteq.Models
{
    public class Product
    {
        public Product()
        {
            Orders = new HashSet<OrderItem>();
        }
        public int ProductId { get; set; }
        public string Image { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }
        public DateTime Date { get; set; }



        public virtual ICollection<OrderItem> Orders { get; set; }

        public static implicit operator List<object>(Product v)
        {
            throw new NotImplementedException();
        }
    }
}
