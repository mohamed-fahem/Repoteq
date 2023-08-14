namespace Repoteq.ViewModel.Orders
{
    public class AddOrderDTO
    {

        public string OrderNumber { get; set; }
        public string CustomerName { get; set; }


        public IEnumerable<OrddrItemDTO> Items { get; set; }

    }

    public class OrddrItemDTO {

        public int ProductId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }


    }

}
