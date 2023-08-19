namespace Repoteq.ViewModel.Orders
{
    public class OrderSearchViewModel
    {   
        public int? OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public double? Total { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
