using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroserviceSagaPattern.Shared.Messages;

namespace MicroserviceSagaPattern.Shared.Events.Order
{
    public class OrderCreatedEvent
    {
        public int OrderId { get; set; }
        public string BuyerId { get; set; }
        public PaymentMessage Payment { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; } = new List<OrderItemMessage>();

    }
}
