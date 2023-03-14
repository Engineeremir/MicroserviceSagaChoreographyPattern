namespace MicroserviceSagaPattern.Order.API.Dtos
{
    public class AddressDto
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
