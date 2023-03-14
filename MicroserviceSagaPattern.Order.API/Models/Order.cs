namespace MicroserviceSagaPattern.Order.API.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string BuyerId { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public OrderStatus OrderStatus { get; set; }
        public string FailMessage { get; set; } = "";
        public Address Address { get; set; }
    }
    public enum OrderStatus
    {
        Suspend,
        Completed,
        Fail
    }
}
