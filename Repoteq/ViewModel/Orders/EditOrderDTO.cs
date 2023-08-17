namespace Repoteq.ViewModel.Orders
{
    public class EditOrderDTO
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public int ProductId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public IEnumerable<OrddrItemDTO> Items { get; set; }
    }
}
