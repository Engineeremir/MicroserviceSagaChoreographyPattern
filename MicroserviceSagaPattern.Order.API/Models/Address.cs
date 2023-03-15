using Microsoft.EntityFrameworkCore;

namespace MicroserviceSagaChoreographyPattern.Order.API.Models
{
    [Owned]
    public class Address
    {
        public string Line { get; set; }
        public string Province { get; set; }
        public string District { get; set; }

    }
}
